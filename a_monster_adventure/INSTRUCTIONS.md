# Godot Team Project Guidelines
## Part 1: Team Workflow
### **The Golden Rules**

**1. One Person Per Scene:** Never edit the same `.tscn` file at the same time as someone else. Merging scenes is nearly impossible.

**2. Close Godot When Switching:** Always close the Godot editor before running `git checkout` to avoid file corruption or import errors.

### **Daily Workflow** (The "Feature Branch" Method)

**1. Start Fresh:** Switch to main (`git checkout main`) and pull changes (`git pull`).

**2. Create Workspace:** Create a branch for your task (`git checkout -b feature/player-jump`).

**3. Work & Save:** Stage (`git add .`) and commit (`git commit -m "Added jump"`) often.

**4. Share:** Push to GitHub (`git push origin feature/player-jump`) and open a Pull Request.

### Emergency Fixes

    • If a scene file (`.tscn`) has a conflict, discard the conflict and re-do changes manually. Do not merge text lines in a scene file.

---

## Part 2: GitHub Features
### **1. Issues** (The "What")

Use this to track bugs and tasks.

#### **How to create an Issue:**

    1. Click the Issues tab at the top of your repo.

    2. Click the green New Issue button.

    3. Title: Be specific (e.g., "Player falls through floor in Level 1").

    4. Description: List steps to reproduce the bug or details about the feature.

    5. Assignees: Click the gear icon on the right to assign it to yourself or a teammate.

    6. Click Submit new issue.

### **2. Projects** (The Kanaban Board)

Use this to see who is doing what (like Trello).

#### **How to set up a Board:**

    1. Click the Projects tab.

    2. Click New Project > Board.

    3. Create columns: `Todo`, `In Progress`, `Done`.

    4. Add Items: Click `+ Add item` at the bottom of a column and search for your existing Issues to add them to the board.

### **3. Pull Requests** (The "How")

Use this to merge your code into the main game.

#### **How to submit a PR:**

    1. Push your branch to GitHub.

    2. You will see a yellow banner on the repo homepage: "feature/branch-name had recent pushes". Click Compare & pull request.

    3. Title: "Added Player Jump".

    4. Description: "Closes #12" (this automatically moves Issue #12 to 'Done').

    5. Reviewers: On the right, click the gear to request a specific teammate to check your work.

    6. Click Create pull request.

## How to Review a PR (For Teammates):

1. Go to the Pull requests tab.

2. Click on a PR to open it.

3. Click the Files changed tab to see exactly what code was modified.

4. Click Review changes (top right).

5. Select Approve (if it looks good) or Request changes (if there are bugs).

6. Once approved, the author can click Merge pull request.

4. Actions (The "Robots")

    ## Advanced: Use this to automate checking your game.

    ### How to start:

        1. Click the Actions tab.

        2. Browse "Workflows".

        3. Look for "Godot Export" actions in the marketplace to automatically build your game when you push code. (This requires setting up a `.yml` file, which is an advanced step for later!).
