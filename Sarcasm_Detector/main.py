import pandas as pd
from sklearn.model_selection import train_test_split
from sklearn.feature_extraction.text import TfidfVectorizer
from sklearn.linear_model import LogisticRegression
from sklearn.pipeline import Pipeline
from sklearn.model_selection import GridSearchCV
from sklearn.metrics import accuracy_score, classification_report
import nltk
from nltk.corpus import stopwords
from nltk.stem import WordNetLemmatizer
from nltk.tokenize import word_tokenize
import tkinter as tk


# Download NLTK data
nltk.download('stopwords')
nltk.download('wordnet')
nltk.download('punkt')
stop_words = set(stopwords.words('english'))
lemmatizer = WordNetLemmatizer()

# Load the dataset from the provided CSV file
file_path = r"C:\Users\maxim\Documents\GitHub\University-Projects\Sarcasm_Detector\train.En.csv"
df = pd.read_csv(file_path)

# Data cleaning: Ensure the 'tweet' column is string and handle any missing values
df['tweet'] = df['tweet'].astype(str).fillna('')

# Preprocess the text
def preprocess_text(text):
    tokens = word_tokenize(text)
    tokens = [word.lower() for word in tokens if word.isalpha()]
    tokens = [lemmatizer.lemmatize(word) for word in tokens if word not in stop_words]
    return ' '.join(tokens)

df['tweet'] = df['tweet'].apply(preprocess_text)

# Split the data into training and testing sets
X_train, X_test, y_train, y_test = train_test_split(df['tweet'], df['sarcastic'], test_size=0.2, random_state=42)

# Create a pipeline
pipeline = Pipeline([
    ('tfidf', TfidfVectorizer(max_features=5000)),
    ('clf', LogisticRegression())
])

# Define hyperparameters for tuning
parameters = {
    'tfidf__max_df': [0.75, 1.0],
    'tfidf__ngram_range': [(1, 1), (1, 2)],
    'clf__C': [0.1, 1, 10]
}

# Use GridSearchCV for hyperparameter tuning
grid_search = GridSearchCV(pipeline, parameters, cv=5, n_jobs=-1)
grid_search.fit(X_train, y_train)

# Evaluate the model
best_model = grid_search.best_estimator_
y_pred = best_model.predict(X_test)

accuracy = accuracy_score(y_test, y_pred)
report = classification_report(y_test, y_pred)

print(f'Accuracy: {accuracy:.2f}')
print('Classification Report:')
print(report)

# Function to predict sarcasm in new text
def predict_sarcasm(text):
    processed_text = preprocess_text(text)
    prediction = best_model.predict([processed_text])
    return 'Sarcasm detected' if prediction[0] == 1 else 'Sarcasm not detected'

# GUI application
def detect_sarcasm():
    input_text = f"{entry1.get()} {entry2.get()}"
    result = predict_sarcasm(input_text)
    result_label.config(text=result)

# Initialize the main window
root = tk.Tk()
root.title("Sarcasm Detection")

# Create and place the widgets
label1 = tk.Label(root, text="Enter first sentence:")
label1.pack()

entry1 = tk.Entry(root, width=50)
entry1.pack()

label2 = tk.Label(root, text="Enter response:")
label2.pack()

entry2 = tk.Entry(root, width=50)
entry2.pack()

detect_button = tk.Button(root, text="Detect Sarcasm", command=detect_sarcasm)
detect_button.pack()

result_label = tk.Label(root, text="")
result_label.pack()

# Start the GUI event loop
root.mainloop()
