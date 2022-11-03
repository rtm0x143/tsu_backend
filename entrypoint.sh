#!/bin/bash

retries=1

until dotnet MovieCatalogBackend.dll; do
        if [ $retries -le  10 ]
        then
          echo "Can't launch MovieCatalogBackend; retry number $retries in 3 secs"
          sleep 3
          retries+=1
        else
          echo "Ended retries to launch MovieCatalogBackend"
          exit
        fi 
done

