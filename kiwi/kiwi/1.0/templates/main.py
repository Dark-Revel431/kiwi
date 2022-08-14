class Main:
	def __init__(self, name):
		self.name = name

	def hi(self):
		print(f"Hi, {self.name}!")


if __name__ == "__main__":
	kiwi = Main("Kiwi")
	kiwi.hi()