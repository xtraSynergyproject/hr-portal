import React from 'react'
import PayrollTab from './components/PayrollTab'
import UserProfile from 'src/views/dashboards/payroll/components/UserProfile'

function PayrollMain() {
  return (
    <div>
      <UserProfile />
      <PayrollTab />
    </div>
  )
}

export default PayrollMain
