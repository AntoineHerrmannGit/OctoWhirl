import pytest
from Core.YFStock import YFStock


def test_yfstock_cac():
    """Test YFStock with CAC ticker"""
    CAC_ticker = "CAC"
    stock = YFStock(CAC_ticker)
    stock.save_to_json()
    
    assert stock.ticker == CAC_ticker
    assert stock.yf_ticker == "^FCHI"
    assert not stock.data.history().empty