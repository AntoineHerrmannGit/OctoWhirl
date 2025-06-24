from datetime import datetime
import os
from Models import Spot, CorporateAction
from .MarketDataArchiver import MarketDataArchiver


class Stock(MarketDataArchiver):
    def __init__(self, ticker: str):
        MarketDataArchiver.__init__(self)
        self.ticker = None
        self.yf_ticker = None
        self.__set_ticker(ticker)
    
    def get_spots(self, start_date: datetime, end_date: datetime, interval: str = '1d') -> list[Spot]:
        raise NotImplementedError()
    
    def get_dividends(self) -> list[CorporateAction]:
        raise NotImplementedError()
        
    def get_splits(self) -> list[CorporateAction]:
        raise NotImplementedError()
    
    def plot(self, start_date: datetime = None, end_date: datetime = None, interval: str = '1d', column: str = 'Close') -> None:
        raise NotImplementedError()
    
    def __set_ticker(self, ticker: str) -> None:
        solution = self.get_solution_root()
        services_project_location = os.path.dirname(self.get_project_location(project="OctoWhirl.Services", root=os.path.dirname(solution)))
        
        config = self.read("appsettings.services.json", root=services_project_location)
        tickers_map = self.get_configuration("Services", "IndexTickerMap", config=config)
        
        if ticker in tickers_map:
            self.yf_ticker = tickers_map[ticker]
        else:
            self.yf_ticker = ticker
      
        self.ticker = ticker