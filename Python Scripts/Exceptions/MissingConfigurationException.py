class MissingConfigurationException(Exception):
    def __init__(self, message: str = None) -> None:
        if message is None:
            message = "Please check config.json"
            
        _message = f"Missing configuration : {message}"
        super().__init__(_message)