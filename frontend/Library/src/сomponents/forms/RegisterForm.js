import React from 'react';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input } from 'antd';

const RegisterForm = ({ setUser, setIsAuthenticated, SetIsRegistered }) => {
  const onFinish = async (values) => {
    console.log('Received values of form: ', values);
    
    // Отправка данных на сервер для регистрации
    try {
      const response = await fetch('https://localhost:7040/User/register', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          email: values.Email,
          userName :values.Username,
          password: values.password,
        }),
      });

      const responseText = await response.text(); // Получаем текст ответа
      console.log('Server response:', responseText);

      if (response.ok) {
        console.log('Registration successful');
        setIsAuthenticated(true);
      } else {
        console.error('Registration failed:');
      }
    } catch (error) {
      console.error('Error during registration:', error);
    }
  };

  return (
    <Form
      name="Registration"
      initialValues={{
        remember: true,
      }}
      style={{
        maxWidth: 360,
      }}
      onFinish={onFinish}
    >
      <div style={{
        display: 'flex',
        justifyContent: 'center',
        marginBottom: '1em',
        fontSize: '1.25em'
      }}>
        Registration
      </div>
      <Form.Item
        name="Email"
        rules={[
          {
            required: true,
            message: 'Please input your Email!',
          },
          {
            type: 'email',
            message: 'Please enter a valid email',
          }
        ]}
      >
        <Input prefix={<UserOutlined />} placeholder="Email" />
      </Form.Item>

      <Form.Item
        name="Username"
        rules={[
          {
            required: true,
            message: 'Please input your Username!',
          }
        ]}
      >
        <Input prefix={<UserOutlined />} placeholder="Username" />
      </Form.Item>

      <Form.Item
        name="password"
        rules={[
          {
            required: true,
            message: 'Please input your Password!',
          },
        ]}
      >
        <Input prefix={<LockOutlined />} type="password" placeholder="Password" />
      </Form.Item>

      <Form.Item>
        <Button block type="primary" htmlType="submit">
          Register
        </Button>
        or <a onClick={() => SetIsRegistered(true)}>Log in</a>
      </Form.Item>
    </Form>
  );
};

export default RegisterForm;
