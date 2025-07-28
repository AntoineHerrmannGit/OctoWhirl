import pytest
import Core


def test_local_node_declarations():
    """Test that each node declares its classes locally and manages dependencies"""
    # Core must expose its classes via local __init__.py
    from Core import ConfigReader, YFStock, Stock, MarketDataArchiver
    
    # Verify classes are accessible
    assert ConfigReader is not None
    assert YFStock is not None
    assert Stock is not None
    assert MarketDataArchiver is not None


def test_local_dependencies_management():
    """Test that each node manages its own dependencies (like Models)"""
    # Core imports Models for its own needs
    # We should be able to access Models via Core
    from Core import Spot, CorporateAction
    
    assert Spot is not None
    assert CorporateAction is not None


def test_explicit_dot_imports():
    """Test that explicit dot imports work for node class declarations"""
    # Explicit import from Core node
    from Core.ConfigReader import ConfigReader as CR_Explicit
    from Core.YFStock import YFStock as YF_Explicit
    
    # These imports must work
    assert CR_Explicit is not None
    assert YF_Explicit is not None
