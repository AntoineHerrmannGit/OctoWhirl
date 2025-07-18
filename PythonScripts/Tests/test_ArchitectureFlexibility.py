import pytest
import Core


def test_local_node_declarations():
    """
    Test que chaque nœud déclare localement ses classes
    et gère ses propres dépendances
    """
    # Core doit exposer ses classes via son __init__.py local
    from Core import ConfigReader, YFStock, Stock, MarketDataArchiver
    
    # Vérifier que les classes sont bien accessibles
    assert ConfigReader is not None
    assert YFStock is not None
    assert Stock is not None
    assert MarketDataArchiver is not None


def test_local_dependencies_management():
    """
    Test que chaque nœud gère ses propres dépendances (comme Models)
    """
    # Core importe Models pour ses propres besoins
    # On doit pouvoir accéder aux Models via Core
    from Core import Spot, CorporateAction
    
    assert Spot is not None
    assert CorporateAction is not None


def test_explicit_dot_imports():
    """
    Test que l'import avec "." fonctionne pour déclarer les classes du nœud
    """
    # Import explicite depuis le nœud Core
    from Core.ConfigReader import ConfigReader as CR_Explicit
    from Core.YFStock import YFStock as YF_Explicit
    
    # Ces imports doivent fonctionner
    assert CR_Explicit is not None
    assert YF_Explicit is not None
