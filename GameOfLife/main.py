import numpy as np
import matplotlib.pyplot as plt
import matplotlib.animation as animation


def randomGrid(N):

    return np.random.choice([0, 1], N * N, p=[0.8, 0.2]).reshape(N, N)


def update(frameNum, img, grid, N):
    newG = grid.copy()
    for i in range(N):
        for j in range(N):


            total = (grid[i, (j - 1) % N] + grid[i, (j + 1) % N] +
                     grid[(i - 1) % N, j] + grid[(i + 1) % N, j] +
                     grid[(i - 1) % N, (j - 1) % N] + grid[(i - 1) % N, (j + 1) % N] +
                     grid[(i + 1) % N, (j - 1) % N] + grid[(i + 1) % N, (j + 1) % N])

            if grid[i, j] == 1:
                if (total < 2) or (total > 3):
                    newG[i, j] = 0
            else:
                if total == 3:
                    newG[i, j] = 1

    img.set_data(newG)
    grid[:] = newG[:]
    return img,


def main():
    N = 50
    grid = randomGrid(N)

    grid[10, 20] = 1
    grid[11, 21] = 1
    grid[12, 21] = 1
    grid[12, 20] = 1
    grid[12, 19] = 1

    fig, ax = plt.subplots()
    img = ax.imshow(grid, interpolation='nearest')
    anim = animation.FuncAnimation(fig, update, fargs=(img, grid, N), frames=120, interval=200, save_count=50)
    plt.show()


if __name__ == '__main__':
    main()
