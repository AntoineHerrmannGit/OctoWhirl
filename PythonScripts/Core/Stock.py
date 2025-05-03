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
        
        config = self.read(root=services_project_location, as_return=True)
        tickers_map = self.get_configuration("IndexTickerMap", config=config)
        
        if ticker not in tickers_map:
            self.ticker = ticker
        else:
            self.ticker = tickers_map[ticker]
        
        if ticker in tickers_map.values():
            self.yf_ticker = [t for t, m in tickers_map.items() if m == ticker][0]
        else:
            self.yf_ticker = ticker