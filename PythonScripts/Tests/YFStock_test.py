import sys
import os

sys.path.append(os.path.abspath(os.path.join(os.path.dirname(__file__), "..")))

from Core import YFStock

CAC_ticker = "CAC"

stock = YFStock(CAC_ticker)
stock.save_to_json()

assert (
    stock.ticker == CAC_ticker
    and stock.yf_ticker == "^FCHI"
    and not stock.data.history().empty
)