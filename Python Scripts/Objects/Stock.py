from datetime import datetime
from Models import Spot, CorporateAction
from .MarketDataArchiver import MarketDataArchiver


class Stock(MarketDataArchiver):
    def __init__(self, ticker: str):
        MarketDataArchiver.__init__(self)
        self.ticker: str = ticker
    
    def get_spots(self, start_date: datetime, end_date: datetime, interval: str = '1d') -> list[Spot]:
        raise NotImplementedError()
    
    def get_dividends(self) -> list[CorporateAction]:
        raise NotImplementedError()
        
    def get_splits(self) -> list[CorporateAction]:
        raise NotImplementedError()
    
    def plot(self, start_date: datetime = None, end_date: datetime = None, interval: str = '1d', column: str = 'Close') -> None:
        raise NotImplementedError()