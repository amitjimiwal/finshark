#!/bin/sh

# CLEAN THE PREVIOUS BUILD
dotnet clean

# BUILD THE PROJECT
dotnet build

# RUN THE PROJECT
dotnet watch run