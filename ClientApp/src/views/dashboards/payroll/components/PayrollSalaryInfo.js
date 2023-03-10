import React from 'react'
import axios from 'axios'
import { useState, useEffect } from 'react'
import Switch from '@mui/material/Switch'
import Box from '@mui/material/Box'
import Card from '@mui/material/Card'
import Typography from '@mui/material/Typography'
import FormControlLabel from '@mui/material/FormControlLabel'

function createData(
  BankAccountNumber,
  FlightTicketFrequency,
  PaymentMode,
  UnpaidLeavesNotInSystem,
  OvertimePaymentType,
  BankBranchIdBranchCode,
  BankIBanNumber
) {
  return (
    BankAccountNumber,
    FlightTicketFrequency,
    PaymentMode,
    UnpaidLeavesNotInSystem,
    OvertimePaymentType,
    BankBranchIdBranchCode,
    BankIBanNumber
  )
}

function PayrollSalaryInfo() {
  // State
  const [info, setInfo] = useState([])
  const [bankAccountNumber, setBankAccountNumber] = useState([])
  const [flightTicketFrequency, setFlightTicketFrequency] = useState([])
  const [paymentMode, setPaymentMode] = useState([])
  const [unpaidLeavesNotInSystem, setUnpaidLeavesNotInSystem] = useState([])
  const [overtimePaymentType, setOvertimePaymentType] = useState([])
  const [bankBranchIdBranchCode, setBankBranchIdBranchCode] = useState([])
  const [bankIBanNumber, setBankIBanNumber] = useState([])

  // ------Switch
  const [takeAttendanceFromTAA, setTakeAttendanceFromTAA] = useState([])
  const [isEmployeeEligibleForEndOfService, setIsEmployeeEligibleForEndOfService] = useState([])
  const [isEmployeeEligibleForFlightTicketsForDependent, setIsEmployeeEligibleForFlightTicketsForDependent] = useState(
    []
  )
  const [isValidateDependentDocumentForBenefit, setIsValidateDependentDocumentForBenefit] = useState([])
  const [isEmployeeEligibleForOvertime, setIsEmployeeEligibleForOvertime] = useState([])
  const [isEmployeeEligibleForFlightTicketsForSelf, setIsEmployeeEligibleForFlightTicketsForSelf] = useState([])
  const [disableFlightTicketProcessingInPayroll, setDisableFlightTicketProcessingInPayroll] = useState([])
  const [isEligibleForSalaryTransferLetter, setIsEligibleForSalaryTransferLetter] = useState([])

  useEffect(() => {
    axios
      .get(`https://webapidev.aitalkx.com/Pay/SalaryInfo/ReadSalaryInfoData`)
      .then(response => {
        setInfo(response.data)
        setBankAccountNumber(response.data[1].BankAccountNumber)
        setFlightTicketFrequency(response.data[1].FlightTicketFrequency)
        setPaymentMode(response.data[1].PaymentMode)
        setUnpaidLeavesNotInSystem(response.data[1].UnpaidLeavesNotInSystem)
        setOvertimePaymentType(response.data[1].OvertimePaymentType)
        setBankBranchIdBranchCode(response.data[1].BankBranchIdBranchCode)
        setBankIBanNumber(response.data[1].BankIBanNumber)

        setTakeAttendanceFromTAA(response.data[1].TakeAttendanceFromTAA)
        setIsEmployeeEligibleForEndOfService(response.data[1].IsEmployeeEligibleForEndOfService)
        setIsEmployeeEligibleForFlightTicketsForDependent(
          response.data[1].IsEmployeeEligibleForFlightTicketsForDependent
        )
        setIsValidateDependentDocumentForBenefit(response.data[1].IsValidateDependentDocumentForBenefit)
        setIsEmployeeEligibleForOvertime(response.data[1].IsEmployeeEligibleForOvertime)
        setIsEmployeeEligibleForFlightTicketsForSelf(response.data[1].IsEmployeeEligibleForFlightTicketsForSelf)
        setDisableFlightTicketProcessingInPayroll(response.data[1].DisableFlightTicketProcessingInPayroll)
        setIsEligibleForSalaryTransferLetter(response.data[1].IsEligibleForSalaryTransferLetter)
      })
      .catch(error => console.log(error))
  })

  return (
    <Card>
    <Box sx={{ display: 'flex', justifyContent:"space-between" }} className='psi_main_box'>
      <Box className='psi_cards left_card' sx={{ p: 3, height: '400px' }}><Box className="box_container">
        <Typography component='h3' variant='p'>
          Pay Group
        </Typography>
        <FormControlLabel checked={takeAttendanceFromTAA} control={<Switch />} label='Take Attendance From TAA' />
        <br />
        <FormControlLabel
          checked={isEmployeeEligibleForEndOfService}
          control={<Switch />}
          label='Is Employee Eligible For End Of Service'
        />
        <br />
        <FormControlLabel
          checked={isEmployeeEligibleForFlightTicketsForDependent}
          control={<Switch />}
          label='Is Employee Eligible For Flight Tickets For Dependent'
        />
        <br />
        <FormControlLabel
          checked={isValidateDependentDocumentForBenefit}
          control={<Switch />}
          label='Is Validate Dependent Document For Benefit '
        />
        <br />
        <Box className='box_three'>
          <Typography sx={{ display: 'flex', m: 2 }}>
            Flight Ticket Frequency:
            {/* {data.FlightTicketFrequency} */}
            <b>{flightTicketFrequency}</b>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            Payment Mode:
            {/* {data.PaymentMode} */}
            <b>{paymentMode}</b>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            Bank Account Number:
            {/* {data.BankAccountNumber} */}
            <b>{bankAccountNumber} </b>
          </Typography>

          <Typography sx={{ display: 'flex', m: 2 }}>
            Unpaid Leaves Not In System:
            {/* {data.UnpaidLeavesNotInSystem} */}
            <b>{unpaidLeavesNotInSystem}</b>
          </Typography>
        </Box>
        </Box>
      </Box>

      <Box className='psi_cards right_card' sx={{ p: 3, height: '400px' }}><Box className="box_container">
        <Typography component='h3' variant='p'>
          Pay Calender
        </Typography>

        <FormControlLabel
          checked={isEmployeeEligibleForOvertime}
          control={<Switch />}
          label='Is Employee Eligible For Overtime'
        />
        <br />
        <FormControlLabel
          checked={isEmployeeEligibleForFlightTicketsForSelf}
          control={<Switch />}
          label='Is Employee Eligible For Flight Tickets For Self'
        />
        <br />
        <FormControlLabel
          checked={disableFlightTicketProcessingInPayroll}
          control={<Switch />}
          label='Disable Flight Ticket Processing In Payroll'
        />
        <br />
        <FormControlLabel
          checked={isEligibleForSalaryTransferLetter}
          control={<Switch />}
          label='Is Eligible For Salary Transfer Letter'
        />
        <br />
        <br />

        <Typography sx={{ display: 'flex', m: 2 }}>
          Overtime Payment Type:
          {/* {data.OvertimePaymentType} */}
          <b>{overtimePaymentType}</b>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          Bank Branch:
          {/* {data.BankBranchIdBranchCode} */}
          <b>{bankBranchIdBranchCode}</b>
        </Typography>

        <Typography sx={{ display: 'flex', m: 2 }}>
          Bank IBAN Number:
          {/* {data.BankIBanNumber} */}
          <b>{bankIBanNumber}</b>
        </Typography>
        </Box>
      </Box>
    </Box></Card>
  )
}

export default PayrollSalaryInfo
