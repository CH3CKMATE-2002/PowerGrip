import { useState } from "react";
import { Button, Form, InputGroup } from "react-bootstrap";
import { BsEye, BsEyeSlash } from "react-icons/bs";
import { ValueComponentProps } from "~/types/components";

interface PasswordInputProps extends ValueComponentProps<string> {
    controlId?: string;
    placeholder?: string;
}

const PasswordInput = ({
    value = '',
    onChange = () => {},
    placeholder = "Enter Your Password",
    controlId
}: PasswordInputProps) => {
    const [hide, setHide] = useState(true);

    const toggleVisibility = () => {
        setHide(!hide);
    };
    
    return (
        <InputGroup>
            <Form.Control
                value={value}
                onChange={e => onChange(e.target.value)}
                type={hide ? 'password' : 'text'}
                placeholder={placeholder}
                id={controlId} />
            
            <Button variant="secondary" onClick={toggleVisibility}>
                {hide ? <BsEye /> : <BsEyeSlash />}
            </Button>
        </InputGroup>
    );
};

export default PasswordInput;