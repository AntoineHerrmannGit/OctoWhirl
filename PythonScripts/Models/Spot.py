from datetime import datetime

class Spot():
    ticker: str
    currency: str
    timestamp: datetime
    open: float
    high: float
    low: float
    close: float
    volume: float
    
    def __init__(self, ticker: str = None, currency: str = None, timestamp: datetime = datetime.now(), open: float = 0, high: float = 0, low: float = 0, close: float = 0, volume: float = 0):
        self.ticker = ticker
        self.currency = currency
        self.timestamp = timestamp
        self.open = open
        self.high = high
        self.low = low
        self.close = close
        self.volume = volume
    
    def to_dict(self):
        return {
            "ticker": self.ticker,
            "timestamp": self.timestamp.isoformat() if isinstance(self.timestamp, datetime) else self.timestamp,
            "open": self.open,
            "high": self.high,
            "low": self.low,
            "close": self.close,
            "volume": self.volume,
            "currency": self.currency
        }