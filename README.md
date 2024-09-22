# PokedexApi

## What is it?
Pokedex API is a simple API that gives some basic info of a specific pokemon, with the option to give the description of the pokemon a fun translation based on Yoda and Shakespeare.
It's developed with ASP.NET Core and has an attached DOCKER file that can be used.

## How to run it?

Method #1 : Visual Studio\n
To launch and debug the application Visual Studio 2022 is needed, with the Web Develop option installed. Docker isn't required and the project will run it without problem (Make sure the Pokedex API is the one being chosen to launch, not Pokedex API Business, Model, etc..)

Method #2 : Docker\n
The project has a docker file that can be run instead, by using the command 'docker build -t pokedex-api .' and, once the image is built, the container can be run with the following command: 'docker run -d -p 8080:8080 -p 8081:8081 pokedex-api'

## What would I change for the PROD version?

A technical modification I would do would be to change (or atleast add it as a separate endpoint) the basic and translated endpoints which currently only allows you to insert only 1 pokemon per call, to allow the insertion of a list of pokemons instead, to allow batch-insertions at once instead of having to call the API n times.

A design modification I would do would be to allow the caller to choose what type of translation the description of the pokemon should have, instead of having the translation be applied arbitrarily.

As it is now, the API should be (and is) open for calls and without requiring any kind of auth, but if we wanted to expand the project an authorization system should be implemented, with Bearer tokens or some similar systems.
