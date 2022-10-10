## Description of the problem
The development of the "JumpForRevit" addin consists of the automation of the reinforcement detail, the process is done by obtaining the curves of the reinforcement and then representing them with lines in a view.
When drawing such lines, for the example provided, strange behavior occurs when trying to move the lines and arcs that correspond to a rebar.

## Obtained result
Lines and arcs are created correctly where the bar is.

![Creation](https://user-images.githubusercontent.com/63598902/194910125-7ff78eda-dbe2-47db-8d7b-637c18a84d80.PNG)

But when trying to move the lines and arcs corresponding to [rebar 2](https://i.stack.imgur.com/HMr3t.png), an error message is thrown.

![Error](https://user-images.githubusercontent.com/63598902/194912346-65361f2b-b4ff-4f6e-b0ca-90ddf9c58bbc.PNG)

Within the addin workflow, creating a Group using the [NewGroup](https://www.revitapidocs.com/2019/8bdb7337-7063-cff8-28a4-958464f2fa5b.htm) method returns a null value.
So it is not possible to get a "BoundingBoxXYZ" from the group.

## Expected result
The expected result would be that you can select all the lines and arcs of the rebar outline by hovering the mouse over the edge of the bar and pressing the "Tab" key until the entire outline is selected.

![Rebar 2](https://user-images.githubusercontent.com/63598902/194911807-d46c8af9-bec3-4951-bb9c-95a0fe4e43ed.PNG)

Once the edges are selected, the lines and arcs are moved out of their host.

![Moved rebar](https://user-images.githubusercontent.com/63598902/194915536-d6e57c0e-132a-4e70-967f-019b94aece4e.PNG)

## Questions
### 1 - Why do lines and arcs meet at points where they do not correspond?

### 2 - Why is it that moving the mouse over the edge of the bar (where the lines are located) and pressing the "Tab" key only selects some of the lines and arcs and not the whole set?
![Rebar](https://user-images.githubusercontent.com/63598902/194913522-d0907506-0cfd-4aac-9208-f898e1c97255.PNG)

### 3 - Why when creating a group with the lines and arcs of bar 2 through the api, does it return a null value?
