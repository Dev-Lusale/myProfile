# Vercel API Setup Guide

## 🚀 Deploy Contact Form API on Vercel

I've created a Vercel serverless function to handle your contact form. Here's how to set it up:

### Step 1: Add Environment Variables to Vercel

1. **Go to your Vercel dashboard**
2. **Select your project** (my-profile-three-theta)
3. **Go to Settings → Environment Variables**
4. **Add these variables**:

```bash
EMAIL_USERNAME=bernardlusale20@gmail.com
EMAIL_PASSWORD=hmciydyldqnaxvec
EMAIL_FROM=bernardlusale20@gmail.com
EMAIL_TO=bernardlusale20@gmail.com
```

### Step 2: Deploy the Updated Code

The files are ready to commit and deploy:

```bash
git add .
git commit -m "Add Vercel serverless function for contact form"
git push
```

### Step 3: Test the Contact Form

After deployment:
1. Visit: https://my-profile-three-theta.vercel.app/contact
2. Fill out the contact form
3. Submit it
4. You should receive an email at bernardlusale20@gmail.com

## 📁 Files Created

### ✅ `api/contact.js`
- Vercel serverless function
- Handles POST requests to `/api/contact`
- Sends notification email to you
- Sends confirmation email to sender
- Includes CORS headers for your domain

### ✅ `package.json`
- Node.js dependencies for Vercel functions
- Includes nodemailer for email sending

### ✅ Updated Configuration
- `appsettings.Production.json` - Points to your Vercel domain
- API service already configured correctly

## 🔧 How It Works

1. **User submits contact form** on your Blazor app
2. **Blazor app calls** `/api/contact` endpoint
3. **Vercel function** receives the request
4. **Nodemailer sends emails** via Gmail SMTP
5. **User gets confirmation**, you get notification

## ✅ Advantages of This Approach

- ✅ **Everything on Vercel** - No need for separate API hosting
- ✅ **Serverless** - Only runs when needed, cost-effective
- ✅ **Fast** - Same domain, no CORS issues
- ✅ **Reliable** - Vercel's infrastructure
- ✅ **Easy to maintain** - All code in one repository

## 🎯 Next Steps

1. **Add environment variables** in Vercel dashboard
2. **Push the code** to GitHub
3. **Test the contact form**
4. **Celebrate** - Your portfolio is complete! 🎉

Your contact form will work perfectly with this setup!