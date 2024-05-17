# CREATED BY Ru and Maksym

import threading
import queue
import random
import time

class JokeFactory:
    def __init__(self, BUFFER_SIZE, NUM_COMEDIANS, SOPHISTICATION):
        self.buffer = queue.Queue(BUFFER_SIZE)
        self.comedians = []
        self.NUM_COMEDIANS = NUM_COMEDIANS
        self.SOPHISTICATION = SOPHISTICATION
        self.lock = threading.Lock()
        self.condition = threading.Condition(self.lock)  # Condition variable based on the lock

    # start(self) -> The function which starts the server and starts appending the comedians to the array
    def start(self):
        self.server_running = True
        for i in range(self.NUM_COMEDIANS):
            comedian = threading.Thread(target=self.tells_joke)
            comedian.start()
            self.comedians.append(comedian)
    # tells_joke -> this function calls create.joke() function and get the Joke (1-100) assign to joke variable
    # then insert this joke into buffer. then wait/sleep for 1 sec
    def tells_joke(self):
        while self.server_running:
            joke = self.create_joke()
            self.insert_joke_in_buffer(joke)
            time.sleep(self.SOPHISTICATION)

    # create_joke -> this function returns a joke between (1-100) in this format: e.g. Joke 14, Joke 67
    def create_joke(self):
        return f"Joke {random.randint(1, 100)}"

    # insert_joke_in_buffer(self, joke) -> Function which is responsible for inserting joke into the buffer
    def insert_joke_in_buffer(self, joke):
        with self.condition:
            while self.buffer.full():
                self.condition.wait()
            self.buffer.put(joke)
            self.condition.notify()

    # get_joke function checks the mode which will be given from user, then assigns values to joke regarding to which mode the user choose
    def get_joke(self, mode):
        self.lock.acquire()
        joke = None
        if self.server_running:
            if mode == "greedy":         # if mode is greedy, then while buffer is not empty jokes will be append into joke array
                jokes = []
                while not self.buffer.empty():
                    jokes.append(self.buffer.get())
                joke = jokes if jokes else None
            elif mode == "fresh":          # if mode is fresh, then only one last joke in the queue will be assign to joke variable
                joke = self.buffer.get() if not self.buffer.empty() else None
            elif mode == "unreceived":     # if mode is unreceived, then take whatever joke that has not been received yet and assign to joke
                joke = self.buffer.get() if not self.buffer.empty() else None
                while not self.buffer.empty():
                    self.buffer.get()
        self.lock.release()
        return joke

    #   stop function stops the server and comedians
    def stop(self):
        self.server_running = False
        with self.condition:
            self.condition.notify_all()
        for comedian in self.comedians:
            comedian.join()

# Assigned numbers to this variables that we used in the code
BUFFER_SIZE = 3
NUM_COMEDIANS = 3
SOPHISTICATION = 1
#  declared the server object with these 3 parameters - BUFFER_SIZE, NUM_COMEDIANS, SOPHISTICATION
server = JokeFactory(BUFFER_SIZE, NUM_COMEDIANS, SOPHISTICATION)

# client(mode) gets the joke regarding to its mode, then print the joke with using join (if list) or just print 1 joke (not list)
def client(mode):
    while True:
        joke = server.get_joke(mode)
        if joke:
            if isinstance(joke, list):
                print(f"{mode} jokes: {', '.join(joke)}")
            else:
                print(f"{mode} joke: {joke}")
        time.sleep(1)

server.start()

# Declaring threads of different jokes
greedy_client = threading.Thread(target=client, args=("greedy",))
fresh_joke_client = threading.Thread(target=client, args=("fresh",))
unreceived_joke_client = threading.Thread(target=client, args=("unreceived",))

# getting user input for the joke's mode
print("Choose a mode to get jokes: greedy (g) - fresh (f) - unreceived (u)")
mode_joke = input()
if mode_joke == 'g':    # if user input is g then start greedy_client
    greedy_client.start()
    time.sleep(5)
    server.stop()
    greedy_client.join()
elif mode_joke == 'f':  # if user input is f then start fresh_client
    fresh_joke_client.start()
    time.sleep(5)
    server.stop()
    fresh_joke_client.join()
elif mode_joke == 'u':  # if user input is u then start unreceived_client
    unreceived_joke_client.start()
    time.sleep(5)
    server.stop()
    unreceived_joke_client.join()
else:
    print("Wrong input")  # if the user's input different then g,f,u then it is wrong input
