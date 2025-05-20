using System;
using System.Reflection;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace FigureReflectionApp
{
    public partial class MainForm : Form
    {
        private Assembly? _shapeAssembly;
        private Type[] _shapeTypes = Array.Empty<Type>();
        private object? _currentInstance;

        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLoadAssembly_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "DLL files (*.dll)|*.dll";
            
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        _shapeAssembly = Assembly.LoadFrom(openFileDialog.FileName);
                        LoadShapeTypes();
                        MessageBox.Show("Assembly loaded successfully!", "Success",
                                         MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading assembly: {ex.Message}", "Error",
                                       MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }


        private void LoadShapeTypes()
        {
            try
            {
                if (_shapeAssembly == null)
                {
                    MessageBox.Show("Assembly not loaded", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                _shapeTypes = _shapeAssembly.GetTypes()
                    .Where(t => t.IsClass &&
                               !t.IsAbstract &&
                               t.GetInterfaces().Any(i => i.Name == "IShape"))
                    .ToArray();

                lstShapes.Items.Clear();
                lstShapes.Items.AddRange(_shapeTypes.Select(t => t.Name).ToArray());

                if (_shapeTypes.Length == 0)
                {
                    MessageBox.Show("No classes implementing IShape found.", "Warning",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading types: {ex.Message}", "Error",
                               MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void lstShapes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstShapes.SelectedIndex == -1) return;

            var selectedType = _shapeTypes[lstShapes.SelectedIndex];
            LoadShapeConstructors(selectedType);
        }

        private void LoadShapeConstructors(Type shapeType)
        {
            pnlConstructor.Controls.Clear();
            pnlMethods.Controls.Clear();
            _currentInstance = null;

            var constructors = shapeType.GetConstructors();
            if (constructors.Length == 0)
            {
                MessageBox.Show("No public constructors found for this type.", "Information",
                             MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var constructor = constructors[0];
            var parameters = constructor.GetParameters();

            Label lblTitle = new Label
            {
                Text = $"Create {shapeType.Name}",
                Dock = DockStyle.Top,
                Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold)
            };
            pnlConstructor.Controls.Add(lblTitle);

            foreach (var param in parameters)
            {
                Label lblParam = new Label { Text = param.Name, Dock = DockStyle.Top };
                TextBox txtParam = new TextBox { Dock = DockStyle.Top, Tag = param.ParameterType };

                pnlConstructor.Controls.Add(txtParam);
                pnlConstructor.Controls.Add(lblParam);
            }

            Button btnCreate = new Button
            {
                Text = "Create Instance",
                Dock = DockStyle.Top
            };
            btnCreate.Click += (s, args) => CreateInstance(shapeType, constructor, parameters);
            pnlConstructor.Controls.Add(btnCreate);
        }

        private void CreateInstance(Type shapeType, ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            try
            {
                object[] paramValues = new object[parameters.Length];
                var textBoxes = pnlConstructor.Controls.OfType<TextBox>().Reverse().ToArray();

                for (int i = 0; i < parameters.Length; i++)
                {
                    try
                    {
                        if (parameters[i].ParameterType == typeof(double[]))
                        {
                            var values = textBoxes[i].Text.Split(',')
                                .Select(s => double.Parse(s.Trim()))
                                .ToArray();
                            paramValues[i] = values;
                        }
                        else
                        {
                            paramValues[i] = Convert.ChangeType(textBoxes[i].Text, parameters[i].ParameterType);
                        }
                    }
                    catch
                    {
                        throw new ArgumentException($"Invalid value for parameter '{parameters[i].Name}'");
                    }
                }

                _currentInstance = constructor.Invoke(paramValues);
                LoadShapeMethods(shapeType);

                var toStringMethod = shapeType.GetMethod("ToString");
                if (toStringMethod != null)
                {
                    string? result = toStringMethod.Invoke(_currentInstance, null) as string;
                    MessageBox.Show($"Created instance:\n{result}", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating instance:\n{ex.InnerException?.Message ?? ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadShapeMethods(Type shapeType)
        {
            pnlMethods.Controls.Clear();

            var methods = shapeType.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => !m.IsSpecialName && m.DeclaringType != typeof(object))
                .ToArray();

            var properties = shapeType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.DeclaringType != typeof(object))
                .ToArray();

            if (methods.Length == 0 && properties.Length == 0)
            {
                MessageBox.Show("No public methods or properties found for this type.",
                              "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Label lblTitle = new Label
            {
                Text = "Methods and Properties",
                Dock = DockStyle.Top,
                Font = new System.Drawing.Font(Font, System.Drawing.FontStyle.Bold)
            };
            pnlMethods.Controls.Add(lblTitle);

            foreach (var prop in properties)
            {
                AddPropertyToPanel(prop);
            }

            foreach (var method in methods)
            {
                AddMethodToPanel(method);
            }
        }

        private void AddPropertyToPanel(PropertyInfo prop)
        {
            GroupBox groupBox = new GroupBox
            {
                Text = $"{prop.Name} (Property)",
                Dock = DockStyle.Top,
                Tag = prop
            };

            Button btnGet = new Button
            {
                Text = "Get Value",
                Dock = DockStyle.Top
            };

            btnGet.Click += (s, args) =>
            {
                try
                {
                    if (_currentInstance == null)
                    {
                        MessageBox.Show("Instance not created", "Error",
                                      MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    object? value = prop.GetValue(_currentInstance);
                    string displayValue = prop.PropertyType == typeof(double[])
                        ? string.Join(", ", (value as double[]) ?? Array.Empty<double>())
                        : value?.ToString() ?? "null";

                    MessageBox.Show($"{prop.Name} = {displayValue}", "Property Value",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error getting property: {ex.Message}", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            groupBox.Controls.Add(btnGet);
            pnlMethods.Controls.Add(groupBox);
        }

        private void AddMethodToPanel(MethodInfo method)
        {
            GroupBox groupBox = new GroupBox
            {
                Text = $"{method.Name}()",
                Dock = DockStyle.Top,
                Tag = method
            };

            FlowLayoutPanel flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoSize = true
            };

            var parameters = method.GetParameters();
            foreach (var param in parameters)
            {
                Label lblParam = new Label { Text = param.Name, AutoSize = true };
                TextBox txtParam = new TextBox { Tag = param.ParameterType, Width = 200 };

                flowPanel.Controls.Add(lblParam);
                flowPanel.Controls.Add(txtParam);
            }

            Button btnExecute = new Button
            {
                Text = "Execute",
                Tag = method
            };
            btnExecute.Click += (s, args) => ExecuteMethod(method, parameters, flowPanel);

            flowPanel.Controls.Add(btnExecute);
            groupBox.Controls.Add(flowPanel);
            pnlMethods.Controls.Add(groupBox);
        }

        private void ExecuteMethod(MethodInfo method, ParameterInfo[] parameters, FlowLayoutPanel panel)
        {
            if (_currentInstance == null)
            {
                MessageBox.Show("Create an instance first!", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                object[] paramValues = new object[parameters.Length];
                var textBoxes = panel.Controls.OfType<TextBox>().ToArray();

                for (int i = 0; i < parameters.Length; i++)
                {
                    paramValues[i] = Convert.ChangeType(textBoxes[i].Text, parameters[i].ParameterType);
                }

                var result = method.Invoke(_currentInstance, paramValues);

                if (method.ReturnType != typeof(void))
                {
                    MessageBox.Show($"Result: {result}", "Method Result",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Method executed successfully!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error executing method: {ex.InnerException?.Message ?? ex.Message}",
                               "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}