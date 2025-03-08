from pynput import keyboard
import threading
import time
import pyautogui

# Flags para controle
w_pressed = False
suspend_script = False

def send_key():
    """Envia continuamente a tecla 'Seta para Cima' enquanto 'W' estiver pressionada."""
    global w_pressed
    pyautogui.keyDown('up')  # Garante que a tecla 'Up' seja enviada imediatamente
    while w_pressed:
        pyautogui.keyDown('up')  # Mantém a tecla pressionada
        time.sleep(0.001)  # Delay reduzido para envio rápido
    pyautogui.keyUp('up')  # Libera a tecla 'Up' ao parar

def on_press(key):
    """Gerencia eventos de pressionamento de tecla."""
    global w_pressed, suspend_script

    try:
        # Alterna suspensão com F4
        if key == keyboard.Key.f4:
            suspend_script = not suspend_script
            print(f"Script {'suspenso' if suspend_script else 'ativado'}.")
            return

        if suspend_script:
            return  # Não faz nada se o script estiver suspenso

        # Quando 'W' é pressionada
        if key.char == 'w' and not w_pressed:
            w_pressed = True
            threading.Thread(target=send_key, daemon=True).start()

    except AttributeError:
        pass

def on_release(key):
    """Gerencia eventos de liberação de tecla."""
    global w_pressed
    try:
        if key.char == 'w':
            w_pressed = False
    except AttributeError:
        pass

# Listener para eventos de teclado
with keyboard.Listener(on_press=on_press, on_release=on_release) as listener:
    print("Pressione F4 para alternar entre ativar e suspender o script.")
    listener.join()
