import pyautogui
import time
import threading

class AutoClicker:
    def __init__(self):
        self.clicking = False
        self.interval = 0.01  # Click interval in seconds
        self.click_thread = threading.Thread(target=self.click_loop)

    def start_clicking(self):
        self.clicking = True
        self.click_thread.start()

    def stop_clicking(self):
        self.clicking = False

    def click_loop(self):
        while self.clicking:
            pyautogui.click()
            time.sleep(self.interval)

auto_clicker = AutoClicker()

while True:
    print("Press 's' to start auto-clicking, 'q' to quit:")
    choice = input().lower()

    if choice == 's':
        auto_clicker.start_clicking()
        print("Auto-clicking started. Press 'q' to quit.")
    elif choice == 'q':
        auto_clicker.stop_clicking()
        break
    else:
        print("Invalid choice. Please try again.")
