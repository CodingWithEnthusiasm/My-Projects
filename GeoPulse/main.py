import tkinter as tk
from tkinter import simpledialog, messagebox,ttk
import folium
import webbrowser
from opencage.geocoder import OpenCageGeocode
import openrouteservice
from PIL import Image, ImageTk


class TodoListApp:
    def __init__(self, root):
        self.root = root
        self.root.title("GeoPulse")
        self.root.geometry("500x300")
        self.center_window()

        self.root = root
        self.root.withdraw()
        self.open_cage_key = self.ask_for_api_key("OpenCage")
        self.open_route_service_key = self.ask_for_api_key("OpenRouteService")
        self.root.deiconify()

        self.map_bounds = {'max_lat': 85, 'min_lat': -85, 'max_lon': 180, 'min_lon': -180}

        self.max_zoom = 10
        self.min_zoom = 2

        self.todo_list = []

        style = ttk.Style()
        style.configure("TButton", padding=6, relief="flat", background="#ccc")
        style.configure("TEntry", padding=6, relief="flat", background="#eee")

        frame = tk.Frame(root)
        frame.pack(side=tk.LEFT, fill=tk.Y)

        self.list_frame = tk.Frame(frame)
        self.list_frame.pack(pady=10)

        self.add_button = ttk.Button(frame, text="Add Item", command=self.add_item)
        self.add_button.pack(side=tk.LEFT, padx=5)

        self.remove_button = ttk.Button(frame, text="Remove Item", command=self.remove_item)
        self.remove_button.pack(side=tk.LEFT, padx=5)

        self.edit_button = ttk.Button(frame, text="Edit Item", command=self.edit_item)
        self.edit_button.pack(side=tk.LEFT, padx=5)

        self.route_button = ttk.Button(frame, text="Build Route", command=self.build_route)
        self.route_button.pack(side=tk.LEFT, padx=5)

        self.quit_button = ttk.Button(frame, text="Quit", command=self.root.destroy)
        self.quit_button.pack(side=tk.LEFT, padx=5)

        ttk.Separator(frame, orient="horizontal").pack(fill="x", pady=5)

        self.map_frame = ttk.Notebook(root, width=300, height=200)
        self.map_frame.pack(pady=10, side=tk.LEFT, expand=True, fill="both")

        self.update_listbox()
        self.create_map()


    def ask_for_api_key(self, service_name):
        return simpledialog.askstring(
        f"{service_name} API Key",
        f"Please enter your {service_name} API Key:",
        parent=self.root)



    def get_and_draw_walking_route(self, coordinates):
        if not all(coordinates):
            messagebox.showerror("Error", "Invalid coordinates. Please ensure all tasks have valid locations.")
            return

        client = openrouteservice.Client(
            self.open_route_service_key)

        coordinates = [(coord[1], coord[0]) for coord in coordinates]

        try:
            route = client.directions(coordinates=coordinates, profile='driving-car', format='geojson')
        except openrouteservice.exceptions.ApiError as e:
            messagebox.showerror("Error", f"Failed to retrieve walking route: {e}")
            return
        try:
            route_coords = [(point[1], point[0]) for point in route['features'][0]['geometry']['coordinates']]
            folium.PolyLine(locations=route_coords, color='blue', weight=5, opacity=0.7).add_to(self.map_widget)


            bounds = self.map_widget.get_bounds()
            self.map_widget.fit_bounds(bounds)
        except Exception as e:
            messagebox.showerror("Error", f"Failed to draw the route: {e}")
            return
        self.update_map_html_file()

    def add_item(self):
        item = simpledialog.askstring("Add Item", "Enter the new item:")
        if item:
            address = simpledialog.askstring("Set Address", f"Enter the address for task '{item}':")
            if address:
                coordinates = self.geocode_address(address)
                if coordinates:
                    lat, lon = coordinates


                    self.create_icon_selector(item, lat, lon)



    def create_icon_selector(self, item, lat, lon):
        self.icon_window = tk.Toplevel(self.root)
        self.icon_window.title("Select Icon")
        self.icon_window.geometry("300x300")


        icon_frame = tk.Frame(self.icon_window)
        icon_frame.pack(expand=True, fill='both')


        self.icon_options = {
            'bus-stop': './Images/bus-stop.png',
            'camera': './Images/camera.png',
            'gas-station': './Images/gas-station.png',
            'gym': './Images/gym.png',
            'hotel': './Images/hotel.png',
            'location': './Images/location.png',
            'park': './Images/park.png',
            'restaurant': './Images/restaurant.png',
            'school': './Images/school.png',
            'shop': './Images/shop.png',
            'theater': './Images/theater.png',
            'pharmacy': './Images/pharmacy.png',
            'home': './Images/home.png',
        }


        def select_icon(icon_name):
            self.selected_icon_name = icon_name
            self.on_icon_selected(item, lat, lon)


        row, col = 0, 0
        max_col = 5

        for icon_name, icon_path in self.icon_options.items():
            try:
                img = Image.open(icon_path)
                img = img.resize((40, 40), Image.ANTIALIAS)
                photo_img = ImageTk.PhotoImage(img)
                btn = tk.Button(icon_frame, image=photo_img, command=lambda name=icon_name: select_icon(name))
                btn.image = photo_img  # Keep a reference
                btn.grid(row=row, column=col, padx=5, pady=5)
                col += 1
                if col >= max_col:
                    col = 0
                    row += 1
            except Exception as e:
                print(f"Error loading {icon_name}: {e}")

    def on_icon_selected(self, item, lat, lon):

        if self.selected_icon_name:
            self.todo_list.append({"task": item, "location": (lat, lon), "icon": self.selected_icon_name})
            self.update_listbox()
            self.update_map()
            self.icon_window.destroy()
            self.create_and_zoom_map(lat, lon)

    def confirm_icon_selection(self, icon_var, icon_window, item, lat, lon):
        icon = icon_var.get()
        self.todo_list.append({"task": item, "location": (lat, lon), "icon": icon})
        self.update_listbox()
        self.update_map()
        icon_window.destroy()

    def geocode_address(self, address):

        geocoder = OpenCageGeocode(self.open_cage_key)

        try:
            result = geocoder.geocode(address)
            if result and len(result):
                lat = result[0]['geometry']['lat']
                lon = result[0]['geometry']['lng']
                return lat, lon
            else:
                messagebox.showerror("Error", "Unable to geocode the address.")
                return None
        except Exception as e:
            messagebox.showerror("Error", f"Error during geocoding: {str(e)}")
            return None

    def remove_item(self):
        selected_indices = self.listbox.curselection()
        if not selected_indices:
            messagebox.showinfo("Remove Items", "Please select at least one task to remove.")
            return


        for index in reversed(selected_indices):
            self.todo_list.pop(index)

        messagebox.showinfo("Item Removed",
                            f"{'Task' if len(selected_indices) == 1 else 'Tasks'} removed successfully.")
        self.update_listbox()
        self.update_map()

    def edit_item(self):
        selected_index = self.listbox.curselection()
        if selected_index:
            index = selected_index[0]
            old_item = self.todo_list[index]

            new_item = simpledialog.askstring("Edit Item", "Enter the new value for the item:",
                                              initialvalue=old_item['task'])
            if new_item:
                address = simpledialog.askstring("Edit Address", f"Enter the new address for task '{new_item}':",
                                                 initialvalue=old_item['location'])
                if address:
                    coordinates = self.geocode_address(address)
                    if coordinates:
                        lat, lon = coordinates


                        self.selected_icon_name = old_item['icon']
                        self.create_icon_selector(new_item, lat, lon)


                        self.todo_list[index] = {"task": new_item, "location": (lat, lon),
                                                 "icon": self.selected_icon_name}
                        self.update_listbox()
                        self.update_map()

    def build_route(self):
        selected_indices = self.listbox.curselection()
        if len(selected_indices) < 2:
            messagebox.showinfo("Route Building", "Please select at least two tasks to build a route.")
            return


        coordinates = [self.todo_list[i]['location'] for i in selected_indices]
        self.get_and_draw_walking_route(coordinates)

    def create_icon_selector(self, item, lat, lon):

        self.icon_window = tk.Toplevel(self.root)
        self.icon_window.title("Select Icon")
        self.icon_window.geometry("300x200")

        icon_frame = tk.Frame(self.icon_window)
        icon_frame.pack(expand=True, fill='both')

        self.icon_options = {
            'bus-stop': './Images/bus-stop.png',
            'camera': './Images/camera.png',
            'gas-station': './Images/gas-station.png',
            'gym': './Images/gym.png',
            'hotel': './Images/hotel.png',
            'location': './Images/location.png',
            'park': './Images/park.png',
            'restaurant': './Images/restaurant.png',
            'school': './Images/school.png',
            'shop': './Images/shop.png',
            'theater': './Images/theater.png',
            'pharmacy': './Images/pharmacy.png',
            'home': './Images/home.png',

        }

        def select_icon(icon_name):
            self.selected_icon_name = icon_name
            self.on_icon_selected(item, lat, lon)

        row, col = 0, 0
        max_col = 5


        for icon_name, icon_path in self.icon_options.items():

            try:
                img = Image.open(icon_path)
                img = img.resize((40, 40), Image.LANCZOS)
                photo_img = ImageTk.PhotoImage(img)
                btn = tk.Button(icon_frame, image=photo_img, command=lambda name=icon_name: select_icon(name))
                btn.image = photo_img
                btn.grid(row=row, column=col, padx=5, pady=5)
                col += 1
                if col >= max_col:
                    col = 0
                    row += 1
            except Exception as e:
                print(f"Error loading {icon_name}: {e}")


    def update_map_html_file(self):

        self.map_html = self.map_widget._repr_html_()
        with open("map.html", "w", encoding="utf-8") as file:
            file.write(self.map_html)
        webbrowser.open("map.html")

    def update_listbox(self):
        for widget in self.list_frame.winfo_children():
            widget.destroy()

        self.listbox = tk.Listbox(self.list_frame, selectmode=tk.EXTENDED, height=10, width=40, font=("Helvetica", 12))
        self.listbox.pack(pady=5)

        for item in self.todo_list:
            self.listbox.insert(tk.END, item['task'])

        ttk.Separator(self.list_frame, orient="horizontal").pack(fill="x", pady=5)
        self.listbox.bind('<Double-Button-1>', self.handle_listbox_double_click)

    def handle_listbox_double_click(self, event):

        index = self.listbox.curselection()

        if index:
            index = index[0]
            item = self.todo_list[index]
            lat, lon = item['location']


            self.create_and_zoom_map(lat, lon)

    def create_and_zoom_map(self, lat, lon):
        zoom_level = 20

        self.map_widget = folium.Map(location=(lat, lon), zoom_start=zoom_level,
                                     max_bounds=True,
                                     max_lat=self.map_bounds['max_lat'],
                                     min_lat=self.map_bounds['min_lat'],
                                     max_lon=self.map_bounds['max_lon'],
                                     min_lon=self.map_bounds['min_lon'],
                                     max_zoom=self.max_zoom,
                                     min_zoom=self.min_zoom)


        for item in self.todo_list:
            icon_url = self.get_icon_url(item['icon'])
            icon = folium.CustomIcon(icon_url, icon_size=(30, 30)) if item.get('icon') else None
            folium.Marker(location=item['location'], popup=item['task'], icon=icon).add_to(self.map_widget)

        self.update_map_html_file()

    def get_icon_url(self, icon_name):
        icon_paths = {
            'bus-stop': 'D:/Images/bus-stop.png',
            'camera': 'D:/Images/camera.png',
            'gas-station': 'D:/Images/gas-station.png',
            'gym': 'D:/Images/gym.png',
            'hotel': 'D:/Images/hotel.png',
            'location': 'D:/Images/location.png',
            'park': 'D:/Images/park.png',
            'restaurant': 'D:/Images/restaurant.png',
            'school': 'D:/Images/school.png',
            'shop': 'D:/Images/shop.png',
            'theater': 'D:/Images/theater.png',
            'pharmacy': 'D:/Images/pharmacy.png',
            'home': 'D:/Images/home.png',

        }
        return icon_paths.get(icon_name, 'path/to/default/icon.png')  # Default icon path

    def on_listbox_select(self, event):
        selected_index = self.listbox.curselection()
        if selected_index:
            index = selected_index[0]
            item = self.todo_list[index]
            self.map_widget.location = item['location']
            self.update_map()

    def create_map(self):
        map_browser = tk.Frame(self.map_frame)
        map_browser.pack(fill="both", expand=True)


        self.map_widget = folium.Map(location=[0, 0], zoom_start=2, max_bounds=True,
                                     max_lat=self.map_bounds['max_lat'], min_lat=self.map_bounds['min_lat'],
                                     max_lon=self.map_bounds['max_lon'], min_lon=self.map_bounds['min_lon'],
                                     max_zoom=self.max_zoom, min_zoom=self.min_zoom)

        for item in self.todo_list:
            folium.Marker(location=item['location'], popup=item['task'], draggable=True).add_to(self.map_widget)

        self.map_html = self.map_widget._repr_html_()
        with open("map.html", "w", encoding="utf-8") as file:
            file.write(self.map_html)


        webbrowser.open("map.html")


        self.inject_js()

    def inject_js(self):
        js_code = """
        var map = L.map('map').setView([0, 0], 2);
        map.setMaxBounds(L.latLngBounds(
            L.latLng({min_lat}, {min_lon}),
            L.latLng({max_lat}, {max_lon})
        ));

        map.on('zoom', function (e) {{
            var currentZoom = map.getZoom();
            if (currentZoom < {min_zoom}) {{
                map.setZoom({min_zoom});
            }} else if (currentZoom > {max_zoom}) {{
                map.setZoom({max_zoom});
            }}
        }});
        """.format(min_lat=self.map_bounds['min_lat'], min_lon=self.map_bounds['min_lon'],
                   max_lat=self.map_bounds['max_lat'], max_lon=self.map_bounds['max_lon'],
                   min_zoom=self.min_zoom, max_zoom=self.max_zoom)


        with open("map.html", "a", encoding="utf-8") as file:
            file.write("<script>{}</script>".format(js_code))

    def update_map(self):

        self.map_widget = folium.Map(location=[0, 0], zoom_start=2,
                                     max_bounds=True,
                                     max_lat=self.map_bounds['max_lat'], min_lat=self.map_bounds['min_lat'],
                                     max_lon=self.map_bounds['max_lon'], min_lon=self.map_bounds['min_lon'],
                                     max_zoom=self.max_zoom, min_zoom=self.min_zoom)



        for item in self.todo_list:
            icon_url = self.get_icon_url(item['icon'])
            icon = folium.CustomIcon(icon_url, icon_size=(30, 30)) if item.get('icon') else None


            popup = folium.Popup(item['task'], parse_html=True)

            folium.Marker(
                location=item['location'],
                icon=icon,
                popup=popup
            ).add_to(self.map_widget)

        self.update_map_html_file()

    def get_walking_route(self, coordinates):
        if not all(coordinates):
            messagebox.showerror("Error", "Invalid coordinates. Please ensure all tasks have valid locations.")
            return None

        client = openrouteservice.Client(self.open_cage_key)  # Replace with your actual API key


        route = client.directions(coordinates, profile='driving-car', format='geojson')
        return route

    def center_window(self):

        screen_width = self.root.winfo_screenwidth()
        screen_height = self.root.winfo_screenheight()

        x_coordinate = int((screen_width - self.root.winfo_reqwidth()) / 2)
        y_coordinate = int((screen_height - self.root.winfo_reqheight()) / 2)

        self.root.geometry("+{}+{}".format(x_coordinate, y_coordinate))


if __name__ == "__main__":
    root = tk.Tk()
    app = TodoListApp(root)
    root.mainloop()
