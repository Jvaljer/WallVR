def clean_logs(file_path):
    with open(file_path, 'r') as file:
        lines = file.readlines()

    # Remove lines above line 31
    if len(lines) > 31:
        lines = lines[31:]

    # Remove lines starting with '# '
    lines = [line for line in lines if not line.startswith(' #')]

    with open(file_path, 'w') as file:
        file.writelines(lines)

file_path = 'log_vr.txt'
clean_logs(file_path)