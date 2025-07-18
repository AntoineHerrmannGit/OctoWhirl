import json
import os
from typing import Any
import logging

from Exceptions import MissingConfigurationException

class ConfigReader: 
    """
    ConfigReader class is just a container of static methods
    """
    logger = logging.getLogger("ConfigReader")
    logger.setLevel(logging.INFO)

    @staticmethod
    def read(filename: str, root: str = None) -> dict[str, Any]:
        try:
            if root is None:
                root = os.path.dirname(__file__)
            appsettings = ConfigReader.find_filepath(filename, root)
            ConfigReader.logger.info(f"Loading configuration from: {appsettings}")
            with open(appsettings, 'r') as f:
                config = json.load(f)
                return config
        except FileNotFoundError:
            raise MissingConfigurationException(f"Configuration file {filename} is missing. Please ensure it exists in the correct directory.")
        except json.JSONDecodeError:
            raise MissingConfigurationException(f"Configuration file {filename} is corrupt or contains invalid JSON.")

    @staticmethod
    def get_configuration(*path: str, config: dict[str, Any] = None) -> str | Any:
        if config is None:
            raise MissingConfigurationException("No configuration provided.")
        result = config
        for section in path:
            if section in result:
                result = result[section]
            else:
                raise MissingConfigurationException(f"Missing configuration section: {'.'.join(path)}")
        return result

    @classmethod
    def find_filepath(cls, filename: str, root: str = None) -> str:
        if filename is None:
            raise ValueError("Filename cannot be None")
        
        current_dir = os.getcwd() if root is None else root
        
        # Validate that the directory exists and is accessible
        if not os.path.exists(current_dir):
            raise FileNotFoundError(f"Directory does not exist: {current_dir}")
        if not os.path.isdir(current_dir):
            raise ValueError(f"Path is not a directory: {current_dir}")
        if not os.access(current_dir, os.R_OK):
            raise FileNotFoundError(f"Cannot access directory: {current_dir}")
        
        visited_dirs = set()
        
        if filename in os.listdir(current_dir):
            return os.path.join(current_dir, filename)
        
        visited_dirs.add(os.path.normpath(current_dir))
        parent_dir = os.path.dirname(current_dir)
        child_dirs = [os.path.join(current_dir, d) for d in os.listdir(current_dir) 
                      if cls.__is_dir(os.path.join(current_dir, d))]
        
        while parent_dir or child_dirs:
            # Search in parent directory
            if parent_dir:
                norm_parent = os.path.normpath(parent_dir)
                if norm_parent not in visited_dirs and os.access(parent_dir, os.R_OK):
                    if filename in os.listdir(parent_dir):
                        return os.path.join(parent_dir, filename)
                    visited_dirs.add(norm_parent)
                
                # Move to next parent, stop at filesystem root
                next_parent = os.path.dirname(parent_dir)
                parent_dir = next_parent if next_parent != parent_dir else None

            # Search in child directories
            new_child_dirs = []
            for dir_path in child_dirs:
                norm_path = os.path.normpath(dir_path)
                if norm_path not in visited_dirs and os.access(dir_path, os.R_OK):
                    if filename in os.listdir(dir_path):
                        return os.path.join(dir_path, filename)
                    visited_dirs.add(norm_path)
                    # Add subdirectories for next iteration
                    new_child_dirs.extend([os.path.join(dir_path, d) for d in os.listdir(dir_path) 
                                         if cls.__is_dir(os.path.join(dir_path, d))])
            
            child_dirs = new_child_dirs
                
        raise FileNotFoundError(f"{filename} not found in any parent or child directories.")

    @staticmethod
    def get_solution_root(solution: str = None) -> str:
        if solution is None:
            solution = "OctoWhirl"
        solution = f"{solution}.sln"
        return ConfigReader.find_filepath(solution)

    @staticmethod
    def get_project_location(project: str, root: str = None) -> str:
        if ".csproj" in project and not project.endswith(".csproj"):
            raise ValueError('Project name must end with ".csproj"')
        else:
            project = f"{project}.csproj"
        if root is None:
            root = os.path.dirname(ConfigReader.get_solution_root())
        dirs = [os.path.join(root, d) for d in os.listdir(root) if ConfigReader.__is_dir(os.path.join(root, d))]
        while dirs:
            new_dirs = []
            for dir in dirs:
                if project in os.listdir(dir):
                    return os.path.join(dir, project)
                else:
                    new_dirs += [os.path.join(dir, d) for d in os.listdir(dir) if ConfigReader.__is_dir(os.path.join(dir, d))]
            dirs = new_dirs
        raise ValueError(f"Service {project} cannot be located from {root}")

    @staticmethod
    def __is_dir(path: str) -> bool:
        return (
            os.path.isdir(path)
            and not os.path.basename(path) in ("bin", "obj", ".vs", ".git", ".idea")
        )

