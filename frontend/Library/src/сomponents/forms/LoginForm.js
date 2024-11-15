import React from 'react';
import { LockOutlined, MailOutlined } from '@ant-design/icons';
import { Button, Checkbox, Form, Input, Flex } from 'antd';
import { useAuth } from '../../context/AuthContext';

const LoginForm = () => {

  const {setIsRegistered,login,errorMessage } = useAuth();

  const onFinish = async (values) => {
    console.log('Received values of form: ', values);

    const loginData = {
      email: values.Email,
      password: values.password,
    };
    await login(loginData);
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
        validateStatus={errorMessage ? 'error' : ''}
        help={errorMessage}
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
        <Input prefix={<MailOutlined />} placeholder="Email" />
      </Form.Item>
      <Form.Item
        name="password"
        validateStatus={errorMessage ? 'error' : ''}
        help={errorMessage}
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
          Log in
        </Button>
        or <a onClick={() => {setIsRegistered(false)}}>Register now!</a>
      </Form.Item>
    </Form>
  );
};
export default LoginForm;