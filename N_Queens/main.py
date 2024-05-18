def check_place(pos, occupied_rows, col):
    for i in range(occupied_rows):
        if pos[i] == col or pos[i] - i == col - occupied_rows or pos[i] + i == col + occupied_rows:
            return False
    return True


class Queens:
    def __init__(self, size):
        self.size = size
        self.solutions = 0
        self.solve()

    def solve(self):
        pos = [-1] * self.size
        self.put_queen(pos, 0)
        print("Solutions found: " + str(self.solutions))

    def put_queen(self, pos, target_row):

        if target_row == self.size:
            self.show_full_board(pos)
            self.solutions += 1
        else:

            for column in range(self.size):
                if check_place(pos, target_row, column):
                    pos[target_row] = column
                    self.put_queen(pos, target_row + 1)

    def show_full_board(self, pos):
        for row in range(self.size):
            line = ""
            for column in range(self.size):
                if pos[row] == column:
                    line += "Q "
                else:
                    line += "0 "
            print(line)
        print("\n")


def main():
    n = int(input("Please enter the amount of queens:"))
    Queens(n)


if __name__ == "__main__":
    main()
