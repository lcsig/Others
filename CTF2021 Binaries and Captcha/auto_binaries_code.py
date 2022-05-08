out_text = bytearray() 
loc1 = 4121 + 2 
loc2 = loc1 + 3  
add = bytearray(b'\x80\xc2')  
sub = bytearray(b'\x80\xea')  
xrr = bytearray(b'\x80\xf2')  
for i in range(0, 914): 
    with open('binaries/bin' + str(i), mode='rb') as file:  
        fileContent = file.read() 
        try: 
            if add in fileContent: 
                out_text.append(fileContent[loc2] - fileContent[loc1]) 
            elif sub in fileContent: 
                out_text.append(fileContent[loc2] + fileContent[loc1]) 
            elif xrr in fileContent: 
                out_text.append(fileContent[loc2] ^ fileContent[loc1]) 
        except: 
            print(i)     
            
          
print(out_text)

