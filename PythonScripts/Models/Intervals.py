from .Enum import Enum

class IntradayInterval(Enum):
    MINUTE = "1m"
    QUARTER = "15m"
    HALFHOUR = "30m"
    HOUR = "1h"
    TWOHOURS = "60m"
    
class ExtradayInterval(Enum):
    DAILY = "1d"
    WEEKLY = "1wk"
    MONTHLY = "1mo"
    YEARLY = "1y"
    
class Interval(IntradayInterval, ExtradayInterval):
    pass