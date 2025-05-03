# OctoWhirl Project

Overall :
---------

This project aims at building a long-term maintainable solution that is not meant open-source. 

To contribute to this project, submit a pull request and follow reviews and comments. Discussion is encouraged. The owner 
    holds full rights to approve, remove and require change.


Developers note :
-----------------

General organisation of the project and coding good practices :

    This project is organized in four main parts:
    
        - Core contains all common structures that will be used in all parts of the code.
            - Models is a particuliar part where models are defined. 
                A model is just a container that carries data and information. It can have fields, attributes and accessors
                but no intelligence.
                If a model is required to be business-oriented and necessitates to have intelligence embded, then it must be 
                written as an extension.
                The only exception is if the model represents a strongly-featured object that carries intelligence. 
                Therefore, this model must not be exposed and treated as internal to the application and technical

        - GUI contains Views, Models and ViewModels which are only responsible of the display of informations.
            - code-behind is allowed to be overridden in ViewModels for very specific tasks like display or calls
            - Views are just a middleware to the ViewModels
            - No intelligence is allowed in the GUI part

        - Services contains services as class or independent services (like APIs) which are fully responsible of the business
            - Services must respect SOLID principles applied to business and use only tools and models defined in Core
            - Services must be thought as independent as possible with each other, minimize legacy code and privilegy 
                maintainance and clear readability.
            - Services may be temporary and must be removable as easily as possible.

        - Tests contains all tests and must follow the same organization than Core, and Services.



Python Scripts :
-----------------

Python scripts are implemented to historize data from the sources.
This implementation must match several constraints :

    - Models:

        - Python models are base models of historizing data. They must match a DBModel on C# side to access database.
        - Unlike C# models (even if it does not match the good practices), Python models must not contain any logics, they are just containers for the data. 

    
Accessing Local Database :
-----------------

The projects assumes that a local database is accessible. The path to access the database is set in the appsettings.json.
