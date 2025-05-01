import finnhub
from datetime import datetime
from Stock import Stock
from Models import Spot, DataSource, Interval

class FHStock(Stock):
    def __init__(self, ticker):
        Stock.__init__(self, ticker)
        self.__init()
        
    def __init(self):
        self.data = None
        self.__client = finnhub.Client(self.get_configuration("FinnHub", "ApiKey"))
    
    def save_to_json(self, override = False) -> None:
        self._save_to_json(DataSource.FINNHUB, override)
    
    def get_spots(self, start_date: datetime, end_date: datetime, interval: Interval = Interval.MINUTE) -> list[Spot]:
        candles = self.__client.stock_candles(self.ticker, interval, start_date, end_date)
        return []

    
raise Exception("The FinnHub Api requires a expensive ApiKey which is not provided yet in config.")