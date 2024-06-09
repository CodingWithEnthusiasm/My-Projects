1. Assembly calculator is a simple single digit calcularo project with multiple operators available which can be runned on any Debian-based system (like Ubuntu) with NASM shell installed on it 

2. Borrow book is a borrow book app writeen in C# the user is needed Visual Studio to run the project, the login and password are admin and 123

3. C++ SDL graphics project which generates a simple 3D figure image and can be runned on any IDE which supports C++ and SDL library

4. The Door&Lock is a code and PDF document with description to my old Arduino project for door lock with code system. It can be runned on simulators like Tinkercad or real Arduino circuit :)

5. GameOfLife is a Conway's Game of life algorithm with an actual animation of a process writeen in Python. Pycharm is required to run it

6. GeneticAlgorithmSolution is an Python app which uses genetic algorithm to solve 8 queens problem. It uses the following elements of the algorithm:

   8-digit integer-value genome

   The fitness function of the form: f(configuration) = 28-number_of_attacks(configuration)

   the genetic operators (roulette wheel selection, two-site crossover, mutation)


7. GeoPulse application is a navigation application created with Python programming language and using OpenCage and OpenRoute services for converting actual addresses into coordinates points and Folium for the usage of interactive map, 

8. JokeFactory is a simple app written in Python which multiple comedians, multiple audience members and bounded joke buffer.

	The server takes the following parameter
		BUFFER_SIZE  (INTEGER)  number of buffer slots, each capable of storing exactly one joke.
		NUM_COMEDIANS (INTEGER) number of processes/threads coming up with jokes
		SOPHISTICATION (INTEGER) a maximum of a random number of seconds that it takes for a comedian to come up with a new joke

	Clients connect to the server in one of the modes
		* greedy mode = take all jokes as they are produced
		* fresh joke mode = always take the most recently produced one
		* take whatever joke that has not been received yet 

9. The N_Queens is an app written in Python which can solve the puzzle of placing n queens on an n x n chessboard such that no two queens attack each other, the user can manually enter the n value. Pycharm is required to run it

10. SimpleAndroidProject is a project writeen in C# with a set of simple features like revert of the string and a calculator which asks the user to choose the app for each operator. Requires an installed Android Stuido to use

11. StudentService project is a project created with a use of .NET framework and MySQL data base and which allows the user to manage different kind of students data. To run it you would need both Visual Studio and MySQL data base installed locally. 

To use it: 

11.1 Import the "degreeproject" folder into your MySQL data base 

11.2 Go to the "Student Service" folder inside the project and in all files with ".cs" extension edit the connstr variable with your actual data base name, user and password

11.3 Open the project solution file in Visual Studio

11.4 The login and password for manager role is UN1@uni.com and 1234, by using it you can add the student users with their own passwords and ids

12. SortingC++ is a C++ project which can be oppened in CodeBlocks. It compares merge sort, heap sort and quick sort algorithms execution speed in nanoseconds for sorted and shuffled arrays with the same input data. These algorithms are tested on arrays from size 100 to 10000, below you also find the brief conclusion for each algorithm from me:

                                                                                    Brute Force 

It is the most primitive algorithm for pattern searching among five present,it has the worst time complexity for comparisions in cases where we have large amount of identical symbols O(m*(n-m+1)) (The example of such cases could be the text "HHHHHHHHHHHHH" and pattern HHH or text GHKGHGGKH and pattern GKH). Best case scenerio for comparison is O(n) and it can be possible only if we the first letter of the pattern is not present in the text. Durring patterrn matching of the book, the algorithm showed second worst result after FSM in terms of average execution time and the worstest in it's growth. The priniple of work of Brute Force supposes that we have to move our pattern to the right by 1 each time and in case if we find a match we need to add 1 to the counter of correct symbols, If there appears a mismatch and counter is bigger then zero, then we set the counter of correct symbols to zero. If counter equals the size of the pattern, it means that we found a match ! 


                                                                                      Sunday

It is simillar to the KMP if comparing the first partsbecause here we also have the library with values. It's best time is O(n/m) and worst is O(mn), among all five alogirthm, this algorithm showed the best results in average execution time and it's growth. As in case of Brute force the worst case appears when all the characters are the same(pattern-OO and text OOOOOOOOOOOO), the best case appears when all the characters in the text and pattern are different (text-AFHKLHRBN and pattern FKL). As KMP it also contains of two parts, in the first part build library in two steps, at first we assign -1 to all the characters from the text that doesn't exist in our pattern, this information again stores in the library array, after that we assign the value to the characters in our pattern, the rightmost character is equal to the size of the pattern and the values of others are equal to their distance from the rightmost character, the repeating characters should be given the same values. This rule includes the rigtmost character. In the second part we use the library in order to make larger jumps in text as in KMP. If we meet the character in the text that doesn't exist in pattern, this jump equals to the size of the pattern in other case it's equal to the value assigned to the character 


                                                                                       KMP

During the pattern searching tests it showed the time results twice quicker then the Brute force and also has second best average time which grows almost as slow as Sunday time, the worst time for comparisons is O(n) and as in Brute force it appears when we have a large amount of mathching character and a mismatch right after them  (TTTTTTTTTTK and patter TTTK for example). The average execution time is pretty close to the Robin Karp's one. The algorithm consists of two parts, in the first part we are building the "library" using prefix and sufix varaibles which will go though pattern and set the values to each letter, this value depends on the size of the matched suffix(the prefix starts from zero and suffix from 1), the are comparing all the possible suffixes and prefixes, if a match found they both move to the right by one if a mismatch is found the prefix goes back by 1 and checks the character again. Library will make the search through the text far more quicker because each time we will meet the characters that exits in our library, we may have a possibility to move more then by 1 step in case if this letter's value doesn't equal zero 

                           
                                                                                        FSM 

This algorithm showed the worst time of execution also at some points the time appeared to decrease and then go up again. On the graph of FSM it can clearly be seen that during the check of the smallest text it was already bigger then 100000 nanoseconds. It's time complexity O(n), though the time complexity of the function which builts the finite automata equals O(n^3*alphabet) where alphabet is a possible number of characters in pattern and text. The main prinicple of this algorithm is that we need to start from the first state of the automata and the fist character in the text and for every further step we are going to check the position of the finite automata. As in KMP algorithm there are more the one part and also exist prefix and suffix variables which are used in order to built automata, so before we will start searching for the pattern we will need to find all posible prefixes in for our Finite automata. In the searching part we are checking if the state of the automata is equal to size of the pattern and if yes then it means that occurence is found


                                                                                     Rabin Karp
It showed almost the same results as Sunday algorithm during the begining but with text size increasing it appeared the Rabin Karp time increases faster the Sunday's and almost as fast as KMP. It's best and usual time complexity is O (n+m) and worst case O(nm). The worst case appears when all characters of pattern and text are same as the hash values of all the substrings of text match with hash value of our pattern. It contains of the two part. The first part is a  the hash function which is the key object of the whole algorithm which calculates the hash of the text and pattern. The second part is the loop which goes through the text using hash data. The hash function makes the whole procedure far more quicker because comparing numbers is a very fast procedure. The main idea of this algorithm is in proper usage of 2 formulas. The first one is being used in order to assign the hash value for the current text and second one is meant to calculate the hash for the further text, remowing the data about the previous character. The very important thing in this formulas is a prime number which prevent the integer overflow 


13. PatternSearchC++ is a project for finding the patterns in text using different algorithms which can be as well oppened using the CodeBlocks. The detailed information about it together with my small research can be found in the SortingAlgorithmMaksymSemko file inside the project folder

14. C++ConsoleBankApp and C++ConsoleTaxApp are simple C++ console projects which implement double quque data structure and imitate a service for collecting taxes and a service for students




 



                              
                              
