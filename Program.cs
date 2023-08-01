using System;
using System.Collections.Generic;

class Program {
	static readonly Random rng = new();
	static int GetRandomInt() => (rng.Next() << 1) ^ rng.Next();

	static void Task1() {
		Func<int, int, int, double> avg = (a, b, c) => ((double)a + b + c) / 3;

		int a = GetRandomInt();
		int b = GetRandomInt();
		int c = GetRandomInt();
		Console.WriteLine($"Середнє арифметичне чисел {a}, {b} і {c} дорівнює");
		Console.WriteLine($"{avg(a, b, c):G15}.");

		Console.WriteLine();
	}

	static double CalculateRPN(string expr) {
		string[] items = expr.Split(' ', StringSplitOptions.RemoveEmptyEntries);
		if (items.Length == 0)
			throw new ArgumentException("Вираз не може бути порожнім.");

		Func<double, double, double> Add, Sub, Mul, Div;
		Add = (a, b) => a + b;
		Sub = (a, b) => a - b;
		Mul = (a, b) => a * b;
		Div = (a, b) => {
			return b is .0 or (-.0) ?
				throw new DivideByZeroException() :
				a / b;
		};

		Stack<double> stack = new();

		foreach (string i in items) {
			if (double.TryParse(i, out double n)) {
				stack.Push(n);
				continue;
			}

			Func<double, double, double> op = i switch {
				"+" => Add,
				"-" => Sub,
				"*" => Mul,
				"/" => Div,
				_ => throw new ArgumentException($"Не вдалося розпізнати елемент: {i}")
			};

			double a = stack.Pop();

			stack.Push(op(stack.Pop(), a));
		}

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

			try {
				Console.WriteLine($"Результат: {CalculateRPN(exp):G15}");
			} catch (ArgumentException e) {
				Console.WriteLine(e.Message);
			} catch (DivideByZeroException) {
				Console.WriteLine("Помилка ділення на нуль.");
			} catch (InvalidOperationException) {
				Console.WriteLine("Вираз помилковий.");
			}
		}
	}

	static void Task3() {
		Func<Func<int>[], double> func = dels => {
			double sum = 0;
			foreach (Func<int> func in dels)
				sum += func();

			return sum / dels.Length;
		};

		Func<int>[] func_arr = new Func<int>[rng.Next(2, 100)];
		Console.WriteLine($"Створюємо масив з {func_arr.Length} делегатів.");
		Console.WriteLine("Кожен із цих делегатів посилається на метод, що повертає випадкове ціле число.");

		Console.WriteLine("Створюємо анонімний метод, що повертає середнє арифметичне чисел, що повертають");
		Console.WriteLine("ці делегати.");

		Array.Fill(func_arr, GetRandomInt);

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
