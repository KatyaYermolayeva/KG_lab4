Пример работы программы (алгоритм Брезенхема):
![image](https://user-images.githubusercontent.com/50108213/165525325-6d316f4f-5b21-407c-9082-d0c162cac8df.png)
![image](https://user-images.githubusercontent.com/50108213/165525401-e6268ccd-81ae-4ea2-9795-4448abe2acb6.png)
Сопроводительные вычисления:
Так как y возрастает быстрее, чем x, поменяем x и y местами. Так как новый y убывает, поставим шаг изменения y равным -1. Шаги алгоритма: 

0) x = -3, y = -2, dx = 9, dy = 3, error = -3
1) -3 < 0:	 x = x + 1 = -2, y = y = -2, error = error + 2 * dy = 3
2) 3 >= 0:	 x = x + 1 = -1, y = y - 1 = -3, error = error + 2 * dy -2 * dx = -9
3) -9 < 0:	 x = x + 1 = 0, y = y = -3, error = error + 2 * dy = -3
4) -3 < 0:	 x = x + 1 = 1, y = y = -3, error = error + 2 * dy = 3
5) 3 >= 0:	 x = x + 1 = 2, y = y - 1 = -4, error = error + 2 * dy -2 * dx = -9
6) -9 < 0:	 x = x + 1 = 3, y = y = -4, error = error + 2 * dy = -3
7) -3 < 0:	 x = x + 1 = 4, y = y = -4, error = error + 2 * dy = 3
8) 3 >= 0:	 x = x + 1 = 5, y = y - 1 = -5, error = error + 2 * dy -2 * dx = -9
9) -9 < 0:	 x = x + 1 = 6, y = y = -5, error = error + 2 * dy = -3
10) -3 < 0:	 x = x + 1 = 7, y = y = -5, error = error + 2 * dy = 3

Целочисленные координаты являются координатами левого нижнего угла ячейки координатной сетки.
