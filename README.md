# Itai's Sudoku Solver

Welcome to my Generic Sudoku Solver! This is A high performance Sudoku Solver written in C# that combines together recursive backtracking with human solving heuristics. This project supports boards from 1x1 up to 25x25, And solves them quickly.

---

## Table of Contents

- [Sudoku Rules](#sudoku-rules)
- [Overview](#overview)
- [Project Features](#project-features)
- [How It Works](#how-it-works)
- [How To Use](#how-to-use)
- [Build and Run](#build-and-run)
- [Testing](#testing)

---



## Sudoku Rules

Sudoku is played on an n x n grid (with n being a perfect square) mostly on 9x9 grids, divided into √n x √n subgrids. The goal is to fill the grid so that each row, each column, and each subgrid contains all symbols exactly once. Empty cells are usually marked with a 0. In my CLI I explained about the sudoku rules also.

---

## Overview

My Sudoku solver uses a mix of human techniques and computer backtracking ( brute force ) , To solve the board. It starts by using human techniques like:

- **Naked Singles:** When a cell has only one option.
- **Hidden Singles:** When a number can only fit in one cell within a row, column, or box.
- **Naked Sets:** When a group of cells share the same set of possibilities.
- **Locked Candidates:** When possibilities are confined to a particular area.

If these methods don't solve the board, the solver switches to backtracking, where it picks a cell with the fewest possibilities and recursively tests each candidate value for this cell until the board is solved,it also uses the human techniques. It does it smart and I will elaborate about it later on.

---

## Project Features

- **Optimized Solving Approach:**  
  Uses simple human strategies (naked singles, hidden singles, naked sets, locked candidates) along with a backtracking algorithm.
  
- **Optimized Backtracking Cell Choosing:**  
  Picking the cell with the fewest possibilities in it,And if there are a more than one cell with the same count of lowest possibilities, it will choose the cell with the highest degreee ( the cell with the maximum count of non zero neighbors ).
- **Optimized Backtracking Cell's Possibility Choosing:**  
  The order of choosing the possibilities is sorted by the affected neighbors of this cell by this possibility ( meaning how many neighbors of that cell that are zero have that possibility in them ).
  
- **Modular Design:**  
  Built with object oriented principles, making it easy to extend or integrate into other projects.

---

## How It Works


### Data Structures
- **Board Representation:**  
  The board is represented as an n x n grid of `Cells<T>` objects. Each cell holds its current value if the cell is permanent, or, a set of the possible candidates for this cell represented by a `HashSet<T>`.


- **State Management:**  
  for each iteration in the backtracking , Before trying to put a certain value It saves the board state in a duplicate of the board matrix - `Cells<T>[,]` and if the guess was wrong then all the values will be copied back to the original board from the copy.

### Solving Process 
1. **Apply Human Techniques:**  
   The solver first uses all the human heuristics that are implemented, To fill in as many cells as possible without guessing.It works in a loop until no changes are made to the board by the heuristics.

2. **Switch to Backtracking:**  
   If these techniques don't solve the entire board then the Backtracking will start, it will use his smart choosing of elements as I explained earlier , And it will use a update board function (for each cell in his neighbors that is not permanent it will remove from his possibilities the main cell value)  and hidden single techniques each time after putting a value.

3. **Repeat Until Solved:**  
   This cycle of applying human techniques and backtracking continues until the board is fully solved or no solution can be found.


---

## How To Use

### Console Interface

- **Input:**  
  You can enter a Sudoku puzzle through the console or by providing a file with a board/boards string (using 0 for empty cells and ascii chars from one up to the board size).
  
- **Output:**  
  The solved board is shown in the console always, and when providing a path file it will create a new file with the boards results represented as strings .

- **Example:** 
![Image](https://github.com/user-attachments/assets/8b3d6420-b970-4fd5-a7e0-97d730e2e4f5)
- **this is the first window that shows when running the program.**
- **you have 4 options:**
1. *Show The rules.*
2. *Insert A board Through the console.*
3. *Insert A board/boards Through A file.*
4. *Exit When finished*

- **Pressing One leads to:**
![Image](https://github.com/user-attachments/assets/6d89d6e0-8c31-4879-803e-bd1173616d1d)
- **After Each Option, The menu pop up will reappear, And you can choose again.**

- **Pressing Two leads to entering a board through the cli:**
1. **First Choose the board string you want And is legal according to the rules**
2. **enter it , and when you want to solve it write "yes"**
3. **The board Can be either Solvable, Unsolvable, or Illegal and you will get an informative response.**
- **Pressing Two and putting a legal and solvable board:**
![Image](https://github.com/user-attachments/assets/e1712fe3-5262-4d67-b421-427bae2e9a9e)
- **Pressing Two and putting a Illegal board:**
![Image](https://github.com/user-attachments/assets/c897ba13-e3fe-4864-919f-cf24e7fbc41d)
- **Pressing Two and putting a Unsolvable board:**
![Image](https://github.com/user-attachments/assets/3796528c-0cf0-4c72-bced-de88d107bbb9)

- **Pressing Three leads to:**
- ![Image](https://github.com/user-attachments/assets/281296e7-6d42-43d5-9755-34519c7a504b)

1. **Entering a valid file with valid board/boards, will show the results in the console and new results file, the result can be the solution/unsolvable/illegal.**
2. **Entering an invalid file will lead to informative message about what is wrong.**

- **Pressing four leads to goodbye message.**

---

## Build and Run

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/itai234/SudokuSolver.git
2. **Unzip the File**
3. **Open With Visual Studio ( 2022 recommended )**
4. **Choose.NET version 8.0 on the project properties**
5. **Click The RUN green button and enjoy**



## Testing

1. **Click on the top of the screen the "test" option**
2. **Click "Run All Tests" And A testing window will pop up.**

