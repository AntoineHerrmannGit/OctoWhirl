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
            appsettings = self.find_filepath("appsettings.json")
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
    
    def find_filepath(self, filename: str):
        if filename is None:
            raise ValueError("Filename cannot be None")
        
        current_dir = os.getcwd()
        if filename in os.listdir(current_dir):
            return os.path.join(current_dir, filename)
        
        parent_dir = os.path.dirname(current_dir)
        child_dirs = [d for d in os.listdir(current_dir) if os.path.isdir(os.path.join(current_dir, d))]
        while [parent_dir] + child_dirs:
            if filename in os.listdir(parent_dir):
                return os.path.join(parent_dir, filename)
            else:
                parent_dir = os.path.dirname(parent_dir)

            for dir in child_dirs:
                if filename in os.listdir(dir):
                    return os.path.join(dir, filename)
            else:
                child_dirs = [d for child in child_dirs for d in os.listdir(child) if os.path.isdir(os.path.join(child, d))]
                
        raise ValueError(f"File {filename} not found in any parent / child directories.")
            