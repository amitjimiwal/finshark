#!/bin/sh

# create a migration taking the name from the first argument
dotnet ef migrations add $1

# apply the migration to the database if user confirms to do so
while true; do
    read -p "Do you wish to apply this migration to the database? You can't undo this action! [Enter y/n]: " yn
    case $yn in
        [Yy]* ) dotnet ef database update ; break;;
        [Nn]* ) echo "Migration update to database canceled." ; exit;;
        * ) echo "Invalid input. Please answer yes (y) or no (n).";;
    esac
done

