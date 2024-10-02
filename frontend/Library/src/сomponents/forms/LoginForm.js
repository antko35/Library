import React from 'react';
import { LockOutlined, UserOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input, Flex } from 'antd';

const LoginForm = ({setUser, setIsAuthenticated, SetIsRegistered}) => {
  const onFinish = async (values) => {
    console.log('Received values of form: ', values);

    const loginData = {
      email: values.Email,
      password: values.password,
    };

    try {
      const response = await fetch('https://localhost:7040/User/login', {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData),
        credentials: 'include', // Для получения и отправки куков
      });

      if (response.ok) {
        // Токен уже сохранен в куке сервером
        setIsAuthenticated(true);
      } else {
        console.error('Login failed');
      }
    } catch (error) {
      console.error('Login failed:', error.message);
    }

  };

  return (
    <Form
      name="login"
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
        justifyContent : 'center',
        marginBottom: '1em',
        fontSize: '1.25em'
        }}>
        Login
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
            message: 'Please enter valid email'
          }
        ]}
      >
        <Input prefix={<UserOutlined />} placeholder="Email" />
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
        <Flex justify="space-between" align="center">
          <Form.Item name="remember" valuePropName="checked" noStyle>
            <Checkbox>Remember me</Checkbox>
          </Form.Item>
        </Flex>
      </Form.Item>

      <Form.Item>
        <Button block type="primary" htmlType="submit">
          Log in
        </Button>
        or <a onClick={() => {SetIsRegistered(false)}}>Register now!</a>
      </Form.Item>
    </Form>
  );
};
export default LoginForm;