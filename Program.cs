using System;
using System.Collections.Generic;

class Program {
	// Випадкові числа для перевірки лямбда-функцій.
	static readonly Random rng = new();
	static int GetRandomInt() => (rng.Next() << 1) ^ rng.Next();

	// Створіть анонімний метод, який приймає в якості параметрів
	// три цілочисельних аргумента і повертає середнє арифметичне цих аргументів.
	static void Task1() {
		// Оголосити анонімний метод, що повертає дійсне число.
		// Перед обчисленням цілочисельні параметри приводяться до дійсних чисел.
		Func<int, int, int, double> avg = (a, b, c) => ((double)a + b + c) / 3;

		// Вивести на екран середнє арифметичне трьох випадкових чисел.
		int a = GetRandomInt();
		int b = GetRandomInt();
		int c = GetRandomInt();
		Console.WriteLine($"Середнє арифметичне чисел {a}, {b} і {c} дорівнює");
		Console.WriteLine($"{avg(a, b, c):G15}.");

		Console.WriteLine();
	}

	// Створіть чотири лямбда оператора для виконання арифметичних дій:
	// (Add - додавання, Sub - віднімання, Mul - множення, Div - розподіл).
	// Кожен лямбда оператор повинен приймати два аргументи
	// і повертати результат обчислення.
	// Лямбда оператор ділення повинен робити перевірку ділення на 0.
	// Написати програму, яка буде виконувати арифметичні дії, зазначені користувачем
	// (По суті = написати калькулятор).
	static double CalculateRPN(string expr) {
		// Розбити текст на числа та оператори.
		string[] items = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (items.Length == 0)
			throw new ArgumentException("Вираз не може бути порожнім.");

		// Оголосити лямбда-функції.
		Func<double, double, double> Add, Sub, Mul, Div;
		Add = (a, b) => a + b;
		Sub = (a, b) => a - b;
		Mul = (a, b) => a * b;
		Div = (a, b) => {
			// Викинути виняток у випадку ділення на ±0.
			return b is .0 or (-.0) ?
				throw new DivideByZeroException() :
				a / b;
		};

		// Числа в виразі зберігаємо в стеку.
		Stack<double> stack = new();

		foreach (string i in items) {
			if (double.TryParse(i, out double n)) {
				// Помістити число в стек.
				stack.Push(n);
				continue;
			}

			// Переконатися, що нечисловий елемент є арифметичним оператором.
			Func<double, double, double> op = i switch {
				"+" => Add,
				"-" => Sub,
				"*" => Mul,
				"/" => Div,
				_ => throw new ArgumentException($"Не вдалося розпізнати елемент: {i}")
			};

			// Зі стеку беремо другий, а потім перший операнди.
			double a = stack.Pop();

			// Обчислити арифметичну операцію та помістити результат у стек.
			stack.Push(op(stack.Pop(), a));
		}

		// Повертаючи результат, переконатися, що стек буде порожній.
		return stack.Count < 1 ?
			stack.Pop() :
			throw new ArgumentException("Вираз помилковий.");
	}
	static void Task2() {
		for (; ; ) {
			Console.WriteLine("Введіть математичний вираз у вигляді зворотного польського запису.");
			Console.WriteLine("Щоб вийти з калькулятора, просто натисніть Enter.");
			string exp = Console.ReadLine();
			if (string.IsNullOrEmpty(exp))
				break;

			// Ловити винятки, які може кидати калькулятор.
			try {
				// Якщо при обчисленні виразу не виникне жодних помилок,
				// його результат буде виведено на екран.
				Console.WriteLine($"Результат: {CalculateRPN(exp):G15}");
			} catch (ArgumentException e) {
				// Цей виняток виникає, якщо обчислити вираз неможливо.
				Console.WriteLine(e.Message);
			} catch (DivideByZeroException) {
				Console.WriteLine("Помилка ділення на нуль.");
			} catch (InvalidOperationException) {
				// Цей виняток виникає при спробі взяти елемент із порожнього стеку.
				Console.WriteLine("Вираз помилковий.");
			}
		}
	}

	// Створіть анонімний метод, який приймає в якості аргументу масив делегатів
	// і повертає середнє арифметичне значень, що повертаються методом,
	// поєднаних з делегатами в масиві.
	// Методи, поєднані з делегатами масиву, повертають випадкове значення типу int.
	static void Task3() {
		// Оголосити функцію, що приймає масив делегатів.
		Func<Func<int>[], double> func = dels => {
			double sum = 0;
			foreach (Func<int> func in dels)
				sum += func();

			return sum / dels.Length;
		};

		// Створити масив делегатів.
		Func<int>[] func_arr = new Func<int>[rng.Next(2, 100)];
		Console.WriteLine($"Створюємо масив з {func_arr.Length} делегатів.");
		Console.WriteLine("Кожен із цих делегатів посилається на метод, що повертає випадкове ціле число.");

		Console.WriteLine("Створюємо анонімний метод, що повертає середнє арифметичне чисел, що повертають");
		Console.WriteLine("ці делегати.");

		// Заповнити масив делегатами, що посилаються на метод, що повертає випадкове ціле число.
		Array.Fill(func_arr, GetRandomInt);

		// Викликати функцію, що приймає масив делегатів, і вивести результат.
		Console.WriteLine($"Результат: {func(func_arr):G15}");

		Console.WriteLine();
	}

	static void Main() {
		Console.Title = "Лабораторна 1";
		Console.OutputEncoding = System.Text.Encoding.UTF8;

		Task1();
		Task2();
		Task3();

		while (Console.KeyAvailable)
			Console.ReadKey(true);
		Console.WriteLine("А на цьому все! Натисніть Enter, щоб продовжити...");
		while (Console.ReadKey(true).Key != ConsoleKey.Enter) {
		}
	}
}
