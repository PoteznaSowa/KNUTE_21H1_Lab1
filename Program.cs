using System;
using System.Collections.Generic;

namespace Y2021H1_Lab1 {
	class Program {
		static void Main() {
			Console.Title = "Лабораторна 1";
			Console.OutputEncoding = System.Text.Encoding.UTF8;

			// Випадкові числа для перевірки анонімних функцій.
			Random random = new Random();

			/*
			 * ================================================================
			 */

			// Створіть анонімний метод, який приймає в якості параметрів
			// три цілочисельних аргумента і повертає середнє арифметичне цих аргументів.
			Func<int, int, int, double> avg = (n1, n2, n3) => (n1 + n2 + n3) / 3.0;
			int n1 = random.Next(-99, 100);
			int n2 = random.Next(-99, 100);
			int n3 = random.Next(-99, 100);
			Console.WriteLine($"n1 = {n1}");
			Console.WriteLine($"n2 = {n2}");
			Console.WriteLine($"n3 = {n3}");
			Console.WriteLine($"avg(n1, n2, n3) = {avg(n1, n2, n3)}");
			Console.WriteLine();

			/*
			 * ================================================================
			 */

			static double CalculateRPN(string exp) {
				// Створіть чотири лямбда оператора для виконання арифметичних дій:
				// (Add - додавання, Sub - віднімання, Mul - множення, Div - розподіл).
				// Кожен лямбда оператор повинен приймати два аргументи
				// і повертати результат обчислення.
				Func<double, double, double> Add = (n1, n2) => n1 + n2;
				Func<double, double, double> Sub = (n1, n2) => n1 - n2;
				Func<double, double, double> Mul = (n1, n2) => n1 * n2;
				Func<double, double, double> Div = (n1, n2) => {
					// Лямбда оператор ділення повинен робити перевірку ділення на 0.
					return n2 == 0.0 || n2 == -0.0 ? throw new DivideByZeroException() : n1 / n2;
				};

				// Написати програму, яка буде виконувати арифметичні дії, зазначені користувачем
				// (По суті = написати калькулятор).

				string[] items = exp.Split(' ', StringSplitOptions.RemoveEmptyEntries);
				if (items.Length == 0) {
					throw new ArgumentException("Вираз не може бути порожнім.");
				}
				Stack<double> stack = new Stack<double>();
				double result;
				foreach (string item in items) {
					if (double.TryParse(item, out double num)) {
						stack.Push(num);
						continue;
					}
					var op = item switch {
						"+" => Add,
						"-" => Sub,
						"*" => Mul,
						"/" => Div,
						_ => throw new ArgumentException($"Не вдалося розпізнати елемент: {item}")
					};
					double n1, n2;
					try {
						n2 = stack.Pop();
						n1 = stack.Pop();
						result = op(n1, n2);
						stack.Push(result);
					} catch (InvalidOperationException) {
						throw new ArgumentException("Помилка у виразі.");
					}
				}
				return stack.Count != 1 ? throw new ArgumentException("Помилка у виразі.") : stack.Pop();
			}

			for (; ; ) {
				Console.WriteLine("Введіть математичний вираз у вигляді зворотного польського запису.");
				Console.WriteLine("Щоб вийти з калькулятора, просто натисніть Enter.");
				string exp = Console.ReadLine();
				if (exp.Length == 0) {
					break;
				}
				try {
					double result = CalculateRPN(exp);
					Console.WriteLine($"Результат: {result:G}");
				} catch (ArgumentException e) {
					Console.WriteLine(e.Message);
				} catch (DivideByZeroException) {
					Console.WriteLine("Помилка ділення на нуль.");
				}
			}
			Console.WriteLine();

			/*
			 * ================================================================
			 */

			// Створіть анонімний метод, який приймає в якості аргументу масив делегатів
			// і повертає середнє арифметичне значень, що повертаються методом,
			// поєднаних з делегатами в масиві.
			// Методи, поєднані з делегатами масиву, повертають випадкове значення типу int.
			Func<Func<int>[], double> func3 = delegs => {
				double sum = 0;
				foreach (var func in delegs) {
					sum += func();
				}
				return sum / delegs.Length;
			};
			Func<int>[] func3_arr = new Func<int>[random.Next(10)];
			Console.WriteLine($"Створюємо масив з {func3_arr.Length} делегатів.");
			Console.WriteLine("Кожен із цих делегатів посилається на метод, що повертає випадкове число в діапазоні [-99;99].");
			for (int i = 0; i < func3_arr.Length; i++) {
				func3_arr[i] = delegate {
					return random.Next(-99, 100);
				};
			}
			Console.WriteLine("Створюємо анонімний метод, що повертає середнє арифметичне чисел, що повертають ці делегати.");
			Console.WriteLine($"Результат: {func3(func3_arr)}");
		}
	}
}
