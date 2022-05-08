import os, requests, subprocess, re
from PIL import Image
import re
import requests
import pytesseract
import subprocess
import os
import gpyocr

s = requests.session()
url = "https://captcha.ctfjo.com/"
s.get(url + "index.php")
while True:
    open('captcha.png', 'wb').write(s.get(url + "captcha.php").content)
    im = Image.open('captcha.png')
    im2 = Image.new("P", im.size, (0, 0, 0))

    for x in range(im.size[1]):
        for y in range(im.size[0]):
            pix = im.getpixel((y, x))
            if pix[0] == 72:
                # and pix[1] == 64 and pix[2] == 161:
                im2.putpixel((y, x), (255, 255, 255))
            else:
                im2.putpixel((y, x), 0)

    # im2 = im2.resize((im.size[0] * 10, im.size[1] * 10), Image.ANTIALIAS)
    im2.save("output.png")
    # im2.convert("RGB")
    captcha = subprocess.getoutput("gocr -C 'A-Za-z' output.png").strip()
    lest = []
    for i in captcha:
        if i.isalpha():
            lest.append(i.upper())
    word = ''.join(lest)
    if len(lest) != 5:
        continue
    response = s.post(url + "check.php", {'answer': word})
    print(word)
    x = 'Total successful breaks:'
    print(response.text[response.text.find(x):(response.text.find(x) + len(x) +
                                               4)])
    if "CTFJO{" in response.text:
        print(response.text)

