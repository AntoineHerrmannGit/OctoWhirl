from datetime import datetime
from Models import CorporateActionType

class CorporateAction():
    ticker: str
    timestamp: datetime
    type: CorporateActionType
    value: float
    
    def __init__(self, ticker: str, timestamp: datetime, type: CorporateActionType, value: float) -> None:
        self.ticker = ticker
        self.timestamp = timestamp
        self.type = type
        self.value = value
        
    def to_dict(self):
        return {
            "ticker": self.ticker,
            "timestamp": self.timestamp.isoformat() if isinstance(self.timestamp, datetime) else self.timestamp,
            "type": self.type,
            "value": self.value
        }