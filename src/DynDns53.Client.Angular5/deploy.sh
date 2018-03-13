#!/bin/bash

ng build --prod --build-optimizer

aws s3 rm s3://dyndns53.myvirtualhome.net --recursive
aws s3 cp ./dist/ s3://dyndns53.myvirtualhome.net --recursive --profile dyndns53-s3


