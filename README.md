# TravellingSalesman
To input data there are 2 options, you can either put a file named input.csv in the root directory of the program, or pass the file path as the first argument to the program when executing it from command line.

The output will by default be output into output.txt, or if a file path is specified in the 2nd argument the output will be saved there. BEWARE! : The file on the output will be overwritten!

## Format of the input.csv file :
- First Line is the number of the city to start in
- Second line is a colon separated list of numbers of cities
- The Third and every following line are routes between cities in the following format : city1,city2,cost
  - The routes are bidirectional so you do not need to define rout for each way
  
## Commented example of the input file :
```csv
1           # The Salesman will start in city with number 1
1,2,3,4,5   # Cities 1,2,3,4 and 5 are defined
1,2,12      # The cost of travel between city 1 and 2 is 12 and vice versa
1,4,15      # The cost of travel between city 1 and 4 is 15 and vice versa, each following line will be parsed as a route
1,5,11
2,1,10
2,3,3
2,4,3
3,1,17
3,2,11
3,4,12
4,2,2
4,5,12
5,1,1
5,3,2
```
