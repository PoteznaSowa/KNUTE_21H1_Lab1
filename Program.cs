using System;
using System.Collections.Generic;

namespace Y2021H1_Lab1 {
	class Program {
		// Випадкові числа для перевірки лямбда-функцій.
		static readonly Random random = new Random();
		static int RandomInt() => random.Next(-0x7FFF_FFFE, 0x7FFF_FFFF);

		// Створіть анонімний метод, який приймає в якості параметрів
		// три цілочисельних аргумента і повертає середнє арифметичне цих аргументів.
		static void Task1() {
			// Оголосити анонімний метод, що повертає дійсне число.
			Func<int, int, int, double> func = (n1, n2, n3) => ((double)n1 + n2 + n3) / 3;

			// Вивести на екран середнє арифметичне трьох випадкових чисел.
			int n1 = RandomInt();
			Console.WriteLine($"n1 = {n1}");
			int n2 = RandomInt();
			Console.WriteLine($"n2 = {n2}");
			int n3 = RandomInt();
			Console.WriteLine($"n3 = {n3}");
			Console.WriteLine($"avg(n1, n2, n3) = {func(n1, n2, n3):G15}");
			Console.WriteLine();
		}

		// Створіть чотири лямбда оператора для виконання арифметичних дій:
		// (Add - додавання, Sub - віднімання, Mul - множення, Div - розподіл).
		// Кожен лямбда оператор повинен приймати два аргументи
		// і повертати результат обчислення.
		// Лямбда оператор ділення повинен робити перевірку ділення на 0.
		// Написати програму, яка буде виконувати арифметичні дії, зазначені користувачем
		// (По суті = написати калькулятор).
		static void Task2() {
			static double CalculateRPN(string exp) {
				// Розбити текст на числа та оператори.
				string[] items = exp.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (items.Length == 0) {
					throw new ArgumentException("Вираз не може бути порожнім.");
				}

				// Оголосити лямбда-функції.
				Func<double, double, double> Add, Sub, Mul, Div;
				Add = (n1, n2) => n1 + n2;
				Sub = (n1, n2) => n1 - n2;
				Mul = (n1, n2) => n1 * n2;
				Div = (n1, n2) => {
					// Викинути виняток у випадку ділення на ±0.
					return n2 == 0.0 || n2 == -0.0 ?
					throw new DivideByZeroException() :
					n1 / n2;
				};

				// Числа в виразі зберігаємо в стеку.
				Stack<double> stack = new Stack<double>();

				foreach (string item in items) {
					if (double.TryParse(item, out double num)) {
						// Помістити число в стек.
						stack.Push(num);
						continue;
					}

					// Переконатися, що нечисловий елемент є арифметичним оператором.
					Func<double, double, double> op = item switch {
						"+" => Add,
						"-" => Sub,
						"*" => Mul,
						"/" => Div,
						_ => throw new ArgumentException($"Не вдалося розпізнати елемент: {item}")
					};

					// Зі стеку беремо другий, а потім перший операнди.
					double n2 = stack.Pop();
					double n1 = stack.Pop();

					// Обчислити арифметичну операцію та помістити результат у стек.
					stack.Push(op(n1, n2));
				}

				// Повертаючи результат, переконатися, що стек буде порожній.
				return stack.Count != 1 ?
					throw new ArgumentException("Вираз помилковий.") :
					stack.Pop();
			}

			for (; ; ) {
				Console.WriteLine("Введіть математичний вираз у вигляді зворотного польського запису.");
				Console.WriteLine("Щоб вийти з калькулятора, просто натисніть Enter.");
				string exp = Console.ReadLine();
				if (exp.Length == 0) {
					break;
				}

				// Ловити винятки, які може кидати калькулятор.
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

		// Створіть анонімний метод, який приймає в якості аргументу масив делегатів
		// і повертає середнє арифметичне значень, що повертаються методом,
		// поєднаних з делегатами в масиві.
		// Методи, поєднані з делегатами масиву, повертають випадкове значення типу int.
		static void Task3() {
			// Оголосити функцію, що приймає масив делегатів.
			Func<Func<int>[], double> func = dels => {
				double sum = 0;
				foreach (Func<int> func in dels) {
					sum += func();
				}
				return sum / dels.Length;
			};

			// Створити масив делегатів.
			Func<int>[] func_arr = new Func<int>[random.Next(2, 100)];
			Console.WriteLine($"Створюємо масив з {func_arr.Length} делегатів.");
			Console.WriteLine("Кожен із цих делегатів посилається на метод, що повертає випадкове ціле число.");

			Console.WriteLine("Створюємо анонімний метод, що повертає середнє арифметичне чисел, що повертають ці делегати.");

			// Заповнити масив делегатами, що посилаються на метод, що повертає випадкове ціле число.
			for (int i = 0; i < func_arr.Length; i++) {
				func_arr[i] = RandomInt;
			}

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

			Console.WriteLine("А на цьому все! Натисніть Enter, щоб продовжити...");
			while (Console.KeyAvailable) {
				Console.ReadKey(true);
			}
			Console.ReadKey(true);
		}
	}
}
