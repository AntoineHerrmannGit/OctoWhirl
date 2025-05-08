import json
import os
from datetime import datetime
from Models import Spot, CorporateAction, DataSource
from .ConfigReader import ConfigReader
from Exceptions import MissingConfigurationException


class MarketDataArchiver(ConfigReader):
    def __init__(self):
        ConfigReader.__init__(self)
        self.ticker = None
        self.currency: str = None
        self.spots: list[Spot] = None
        self.spots_intraday: list[Spot] = None
        self.dividends: list[CorporateAction] = None
        self.splits: list[CorporateAction] = None
    
    def _save_to_json(self, source: DataSource, override: bool = False) -> None:
        """
        Save stock data to a JSON file. 
        Enrich each market data file with new data and overrides with new data if specified.
        
        :param override: If True, override existing data. Default is False.
        """
        if "LocalBasePath" not in self.configuration:
            MissingConfigurationException("LocalBasePath")
        
        solution_root = os.path.dirname(self.get_solution_root())
        database_config_root = os.path.dirname(self.get_project_location(root=solution_root, project="OctoWhirl.Services"))
        database_config = self.read(root=database_config_root, as_return=True)
        local_base_path = os.path.join(solution_root, self.get_configuration("DataBase", "LocalBasePath", config=database_config), source)
        
        
        stocks_intraday_filename = self.get_configuration("DataBase", "DataBaseFileNames", "StockIntraday", config=database_config)
        stocks_daily_filename = self.get_configuration("DataBase", "DataBaseFileNames", "StockDaily", config=database_config)
        dividends_filename = self.get_configuration("DataBase", "DataBaseFileNames", "Dividends", config=database_config)
        splits_filename = self.get_configuration("DataBase", "DataBaseFileNames", "Splits", config=database_config)
        
        self.__save_detailed_stock(spots=self.spots, root=local_base_path, filename=os.path.join(local_base_path, self.__get_file_key(stocks_intraday_filename)), override=override)
        self.__save_daily_stock(spots=self.spots_intraday, root=local_base_path, filename=os.path.join(local_base_path, self.__get_file_key(stocks_daily_filename)), override=override)
        self.__save_dividends(dividends=self.dividends, root=local_base_path, filename=os.path.join(local_base_path, self.__get_file_key(dividends_filename)), override=override)
        self.__save_splits(splits=self.splits, root=local_base_path, filename=os.path.join(local_base_path, self.__get_file_key(splits_filename)), override=override)
    
    def __get_file_key(self, marketdata: str) -> str:
        """
        Generate a file key for the given ticker.
        
        :param marketdata: Market data type ('stocks', 'dividends', 'actions').
        :return: File key for the ticker.
        """
        return os.path.join(marketdata, f"{self.ticker}.json")

    def __check_file_exists(self, file_path: str) -> bool:
        """
        Check if a file exists.

        :param file_path: Path to the file.
        :return: True if the file exists, False otherwise.
        """
        return os.path.exists(file_path) and os.path.isfile(file_path)
    
    def __create_file(self, file_path: str, root: str = None) -> None:
        """
        Create a new file for the given market data type.

        :param file_path: Path to the file.
        :param marketdata: Market data type ('stocks', 'dividends', 'actions').
        """
        marketdata = os.path.split(os.path.dirname(file_path))[1]
        if not os.path.exists(os.path.join(root, marketdata)):
            os.makedirs(os.path.join(root, marketdata))
        if not os.path.exists(file_path):
            with open(file_path, 'w') as f:
                json.dump({}, f)
    
    def __merge_spot_data(self, new_spots: list[Spot], old_spots: list[Spot], override: bool) -> list[Spot]:
        new_spots_dict = {spot.timestamp: spot for spot in new_spots}
        old_spots_dict = {spot.timestamp: spot for spot in old_spots}
        all_dates = list(set(list(new_spots_dict.keys()) + list(old_spots_dict.keys())))
        
        merged_data = []
        if override:
            for date in all_dates:
                if date in new_spots_dict:
                    merged_data.append(new_spots_dict[date])
                elif date in old_spots_dict:
                    merged_data.append(old_spots_dict[date])
        else:
            for date in all_dates:
                if date in old_spots_dict:
                    merged_data.append(old_spots_dict[date])
                elif date in new_spots_dict:
                    merged_data.append(new_spots_dict[date])
                    
        return sorted(merged_data, key=lambda x: x.timestamp)
    
    def __merge_corporate_actions(self, new_ca: list[CorporateAction], old_ca: list[CorporateAction], override: bool) -> list[CorporateAction]:
        new_ca_dict = {ca.timestamp: ca for ca in new_ca}
        old_ca_dict = {ca.timestamp: ca for ca in old_ca}
        all_dates = list(set(list(new_ca_dict.keys()) + list(old_ca_dict.keys())))
        
        merged_ca = []
        if override:
            for date in all_dates:
                if date in new_ca_dict:
                    merged_ca.append(new_ca_dict[date])
                elif date in old_ca_dict:
                    merged_ca.append(old_ca_dict[date])
        else:
            for date in all_dates:
                if date in old_ca_dict:
                    merged_ca.append(old_ca_dict[date])
                elif date in new_ca_dict:
                    merged_ca.append(new_ca_dict[date])
                    
        return sorted(merged_ca, key=lambda action: action.timestamp)
    
    def __save_detailed_stock(self, spots: list[Spot], root: str, filename: str, override: bool) -> None:
        if not spots:
            return 
        
        if not self.__check_file_exists(filename):
            self.__create_file(filename, root=root)
        
        with open(filename, "r") as file:
            in_base_data = json.load(file)
            in_base_spots = [
                Spot(
                    timestamp=datetime.fromisoformat(spot["timestamp"]),
                    ticker=spot["ticker"],
                    currency=spot["currency"],
                    open=spot["open"],
                    high=spot["high"],
                    low=spot["low"],
                    close=spot["close"],
                    volume=spot["volume"]
                ) for spot in in_base_data
            ]
        
        data_to_save = self.__merge_spot_data(spots, in_base_spots, override)
        with open(filename, "w") as file:
            file.truncate(0)
            json.dump([spot.to_dict() for spot in data_to_save], file)
    
    def __save_daily_stock(self, spots: list[Spot], root: str, filename: str, override: bool) -> None:
        if not spots:
            return
        
        if not self.__check_file_exists(filename):
            self.__create_file(filename, root=root)
        
        with open(filename, "r") as file:
            in_base_data = json.load(file)
            in_base_spots = [
                Spot(
                    timestamp=datetime.fromisoformat(spot["timestamp"]),
                    ticker=spot["ticker"],
                    currency=spot["currency"],
                    open=spot["open"],
                    high=spot["high"],
                    low=spot["low"],
                    close=spot["close"],
                    volume=spot["volume"]
                ) for spot in in_base_data
            ]
        
        data_to_save = self.__merge_spot_data(spots, in_base_spots, override)
        with open(filename, "w") as file:
            file.truncate(0)
            json.dump([spot.to_dict() for spot in data_to_save], file)
        
    def __save_splits(self, splits: list[CorporateAction], root: str, filename: str, override: bool) -> None:
        if not splits:
            return 
        
        if not self.__check_file_exists(filename):
            self.__create_file(filename, root=root)
        
        with open(filename, "r") as file:
            in_base_data = json.load(file)
            in_base_splits = [
                CorporateAction(
                    ticker=split["ticker"],
                    timestamp=datetime.fromisoformat(split["timestamp"]),
                    type=split["type"],
                    value=split["value"]
                ) for split in in_base_data
            ]
        
        data_to_save = self.__merge_corporate_actions(splits, in_base_splits, override)
        with open(filename, "w") as file:
            file.truncate(0)
            json.dump([ca.to_dict() for ca in data_to_save], file)
        
    def __save_dividends(self, dividends: list[CorporateAction], root: str, filename: str, override: bool) -> None:
        if not dividends:
            return
        
        if not self.__check_file_exists(filename):
            self.__create_file(filename, root=root)
        
        with open(filename, "r") as file:
            in_base_data = json.load(file)
            in_base_splits = [
                CorporateAction(
                    ticker=dividend["ticker"],
                    timestamp=datetime.fromisoformat(dividend["timestamp"]),
                    type=dividend["type"],
                    value=dividend["value"]
                ) for dividend in in_base_data
            ]
        
        data_to_save = self.__merge_corporate_actions(dividends, in_base_splits, override)
        with open(filename, "w") as file:
            file.truncate(0)
            json.dump([ca.to_dict() for ca in data_to_save], file)
        
