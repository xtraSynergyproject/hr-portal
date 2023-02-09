// ** React Imports
import { useState } from 'react'

// ** MUI Imports
import Tab from '@mui/material/Tab'
import TabList from '@mui/lab/TabList'
import TabPanel from '@mui/lab/TabPanel'
import TabContext from '@mui/lab/TabContext'
import PayrollSalaryInfo from './PayrollSalaryInfo'
import PayrollPaySlip from './PayrollPaySlip'

const PayrollTab = () => {
  // ** State
  const [value, setValue] = useState('1')

  const handleChange = (event, newValue) => {
    setValue(newValue)
  }

  return (
    <TabContext value={value}>
      <TabList centered fullWidth onChange={handleChange} aria-label='centered tabs example'>
        <Tab value='1' label='Salary Info' />
        <Tab value='2' label='Pay Slip' />
      </TabList>

      <TabPanel value='1'>
        <PayrollSalaryInfo />
      </TabPanel>

      <TabPanel value='2'>
        <PayrollPaySlip />
      </TabPanel>
    </TabContext>
  )
}

export default PayrollTab
