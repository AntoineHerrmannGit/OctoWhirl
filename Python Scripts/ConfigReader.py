import json
import os
from typing import Any
from Exceptions import MissingConfigurationException

class ConfigReader():
    def __init__(self):
        self.configuration = None
        self.read()
    
    def read(self) -> dict[str, Any]:
        try:
            appsettings = os.path.join(os.getcwd(), 'appsettings.json')
            with open(appsettings, 'r') as f:
                self.configuration = json.load(f)
            return self.configuration
        
        except (FileNotFoundError, json.JSONDecodeError) as e:
            raise MissingConfigurationException("Configuration file may be missing or corrupt")

    def get_configuration(self, *path: str) -> str | Any:
        if self.configuration is None:
            raise MissingConfigurationException("No configuration found.")
        
        result = self.configuration
        for section in path:
            if section in result:
                result = result[section]
            else:
                raise MissingConfigurationException(section)
        
        return result
            