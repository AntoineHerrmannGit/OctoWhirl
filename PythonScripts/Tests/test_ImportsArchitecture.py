import pytest
from Core import ConfigReader, Stock, YFStock, MarketDataArchiver
from Core.ConfigReader import ConfigReader as CR
from Core.Stock import Stock as S
from Core.YFStock import YFStock as YFS
from Core.MarketDataArchiver import MarketDataArchiver as MDA


def test_import_from_core_package():
    """Test that we can import directly from Core package"""
    # Imports are already tested at module level above
    assert hasattr(ConfigReader, 'read')
    assert hasattr(Stock, '__init__')
    assert hasattr(YFStock, 'get_spots')
    assert hasattr(MarketDataArchiver, '__init__')


def test_specific_absolute_imports():
    """Test specific absolute imports with dot notation"""
    # Verify that classes are properly imported via explicit paths
    assert hasattr(CR, 'read')
    assert hasattr(S, '__init__')
    assert hasattr(YFS, 'get_spots')
    assert hasattr(MDA, '__init__')


def test_class_inheritance_works():
    """Test that inheritance works with strict imports"""
    # We just test that classes are properly linked
    assert issubclass(YFStock, Stock)


def test_node_dependency_management():
    """Test que chaque nœud gère ses dépendances (Core gère Models)"""
    # Core a importé Models pour ses besoins, donc on peut y accéder via Core
    from Core import Spot, CorporateAction
    
    assert Spot is not None
    assert CorporateAction is not None


def test_local_exceptions_imports():
    """Test que Exceptions imports work when imported explicitly"""
    # Import only what we need, when we need it
    from Exceptions import MissingConfigurationException
    
    assert MissingConfigurationException is not None
