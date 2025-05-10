import yfinance as yf
import matplotlib.pyplot as plt
from datetime import datetime, timedelta
from pandas import DataFrame
from Models import Spot, CorporateAction, CorporateActionType
from Models import Interval, IntradayInterval, DataSource
from .Stock import Stock

class YFStock(Stock):
    def __init__(self, ticker: str) -> None:
        Stock.__init__(self, ticker)
        self.data = None
        self.__init()

    def save_to_json(self, override = False) -> None:
        self._save_to_json(DataSource.YFINANCE, override)

    def plot(self, start_date: datetime = None, end_date: datetime = None, interval: Interval = Interval.MINUTE, column: str = 'Close') -> None:
        """
        Plot the stock data.
        
        :param start_date: Start date for the data to be plotted.
        :param end_date: End date for the data to be plotted.
        :param interval: Interval for the data ('1m', '2m', '5m', '15m', '30m', '60m', '90m', '1d', '1wk', '1mo').
        """
        if start_date is None:
            start_date = datetime.now() - timedelta(days=30)
        if end_date is None:
            end_date = datetime.now()
        if interval is None:
            interval = '1d'
        if column is None:
            column = 'Close'
            
        if self.data is None:
            self.__get()
            
        data = self.data.history(period='max', start=start_date, end=end_date, interval=interval)[column]
        
        if self.data is not None:
            plt.figure()
            plt.plot(data.index, data.values, c='blue', label=self.ticker)
            plt.title(f'{self.ticker} Stock Price')
            plt.xlabel('Date')
            plt.ylabel('Close')
            plt.legend()
            plt.show()
        else:
            print("No data to plot. Please fetch data first.")

    def get_spots(self, start_date: datetime, end_date: datetime, interval: Interval = Interval.MINUTE) -> list[Spot]:
        """
        Fetch stock data for the given ticker from Yahoo Finance.
        
        :param start_date: Start date for the data to be fetched.
        :param end_date: End date for the data to be fetched.
        :param interval: Interval for the data.
        :param source: Source for the data
        :return: Stock data list of Spot.
        """
        if self.data is None:
            self.__get_ticker()
            
        if interval in IntradayInterval:
            self.spots_intraday = self.__convert_stocks_dataframe_to_Spots(self.data.history(period='max', start=start_date, end=end_date, interval=interval))
        else:
            self.spots = self.__convert_stocks_dataframe_to_Spots(self.data.history(period='max', start=start_date, end=end_date, interval=interval))
        
        return self.spots

    def get_dividends(self) -> list[CorporateAction]:
        if self.data is None:
            self.__get_ticker()
        
        dividends = DataFrame(self.data.dividends)
        if dividends.empty or len(dividends) == 0:
            return []
        
        if "Value" not in dividends:
            dividends["Value"] = dividends["Dividends"]
        
        self.dividends = self.__convert_dataframe_to_corporate_actions(dividends, CorporateActionType.DIVIDEND)
        return self.dividends

    def get_splits(self) -> list[CorporateAction]:
        if self.data is None:
            self.__get_ticker()
        
        splits = DataFrame(self.data.splits)
        if splits.empty or len(splits) == 0:
            return []
        
        if "Value" not in splits:
            splits["Value"] = splits["Stock Splits"]
        
        self.splits = self.__convert_dataframe_to_corporate_actions(splits, CorporateActionType.SPLIT)
        return self.splits
        
    def __init(self) -> None:
        self.__get_ticker()
        self.currency = self.get_currency()
        self.spots = self.get_spots(start_date=datetime.now() - timedelta(days=30), end_date=datetime.now(), interval=Interval.DAILY)
        self.spots_intraday = self.get_spots(start_date=datetime.now() - timedelta(days=8), end_date=datetime.now(), interval=Interval.MINUTE)
        self.dividends = self.get_dividends()
        self.splits = self.get_splits()
    
    def get_currency(self) -> str:
        if self.data is None:
            self.__get_ticker()
        if "currency" in self.data.info:
            return self.data.info["currency"]
        return None
    
    def __get_ticker(self) -> yf.Ticker:
        """
        Fetch stock data for the given tickers from source.
        """
        if self.data is None:
            self.data = yf.Ticker(self.yf_ticker)
        return self.data
    
    def __convert_stocks_dataframe_to_Spots(self, stock_data: DataFrame) -> list[Spot]:
        if stock_data is None or stock_data.empty or len(stock_data) == 0:
            return []
        
        if "Datetime" not in stock_data:
            stock_data["Datetime"] = stock_data.index
            
        stocks = []
        for _, row in stock_data.iterrows():
            stocks.append(Spot(
                ticker = self.ticker,
                timestamp = row["Datetime"],
                currency=self.data.info["currency"],
                open = row["Open"],
                high = row["High"],
                low = row["Low"],
                close = row["Close"],
                volume = row["Volume"]
            ))
        return sorted(stocks, key=lambda spot: spot.timestamp)
    
    def __convert_dataframe_to_corporate_actions(self, corporate_actions: DataFrame, action_type: CorporateActionType) -> list[CorporateAction]:
        if corporate_actions is None or corporate_actions.empty or len(corporate_actions) == 0:
            return []
        
        if "Datetime" not in corporate_actions:
            corporate_actions["Datetime"] = corporate_actions.index
            
        corpactions = []
        for _, row in corporate_actions.iterrows():
            corpactions.append(CorporateAction(
                ticker=self.ticker,
                timestamp=row["Datetime"],
                type=action_type,
                value=row["Value"]
            ))
        return sorted(corpactions, key=lambda action: action.timestamp)
