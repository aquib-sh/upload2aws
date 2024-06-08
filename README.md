## Example of how to upload a file using this API

Here is a Python code to do it

```python
import requests
import json
import base64

# Replace with your data and content type
raw_data = open("cute_duck.jpg", "rb").read()

data = base64.b64encode(raw_data).decode()

payload = {
  "mimetype":"image/jpg",
  "data":data
}

url = f"http://localhost:5268/storage/upload?path=images/bird"

# Send POST request with data and content type
response = requests.post(url, data=json.dumps(payload), headers={"Content-Type": "application/json"})

# Check for successful response
if response.status_code == 200:
  # Response contains the uploaded file name
  filename = response.text
  print(f"File uploaded successfully! Filename: {filename}")

else:
  print(f"Error uploading file. Status code: {response.status_code}")
  print(response.content)
```
